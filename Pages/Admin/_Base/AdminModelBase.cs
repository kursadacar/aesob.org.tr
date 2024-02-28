using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using aesob.org.tr.Models;
using aesob.org.tr.Pages.Content.Base;
using aesob.org.tr.Services;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace aesob.org.tr
{
    public interface IAdminModel
    {
        object NewItem { get; }

        List<object> ItemList { get; }

        int AvailableEntityID { get; }

        List<string> FilesInDirectory { get; }

        IFormFile UploadedFile { get; set; }

        string CurrentQuery { get; }
    }

    public abstract class AdminModelBase<T> : AesobModelBase, IAdminModel, INavigablePage where T : class, IAesobEntity, new()
    {
        private List<object> _itemList;

        public int MaxPerPage
        {
            get
            {
                return 16;
            }
        }

        public string ContentPageName
        {
            get
            {
                return "Admin/" + GetType().Name.Replace("Model", "");
            }
        }

        public int MaxPages { get; private set; }

        public int CurrentPageIndex { get; private set; }

        [BindProperty]
        public T NewItem { get; set; }

        [BindProperty]
        public List<T> ItemList { get; set; }

        [BindProperty]
        public int AvailableEntityID { get; set; }

        [BindProperty]
        public int DeletedItemID { get; set; }

        object IAdminModel.NewItem
        {
            get
            {
                return NewItem;
            }
        }

        List<object> IAdminModel.ItemList
        {
            get
            {
                if (_itemList == null && ItemList != null)
                {
                    _itemList = new List<object>();
                    foreach (T item in ItemList)
                    {
                        _itemList.Add(item);
                    }
                }
                return _itemList;
            }
        }

        public List<string> FilesInDirectory { get; private set; }

        public string ContentParameterName
        {
            get
            {
                return "pageIndex";
            }
        }

        public string CurrentQuery { get; private set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public AdminModelBase(AesobDbContext context)
            : base(context)
        {
        }

        public IActionResult OnGet(string query = null, int pageIndex = 0)
        {
            if (!base.User.Identity.IsAuthenticated)
            {
                base.HttpContext.SignOutAsync("Cookies");
                return new RedirectToPageResult("/Index");
            }
            CurrentQuery = query;
            CurrentPageIndex = pageIndex;
            DbSet<T> collection = _context.Set<T>();
            List<T> itemsToDelete = new List<T>();
            if (!string.IsNullOrEmpty(query))
            {
                List<T> itemsCopy = collection.ToList();
                foreach (T item2 in itemsCopy)
                {
                    bool isItemFiltered = true;
                    PropertyInfo[] properties = item2.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        object propValue = property.GetValue(item2);
                        if (propValue != null && propValue.ToString().ToLower().Contains(query.ToLower()))
                        {
                            isItemFiltered = false;
                        }
                    }
                    if (isItemFiltered)
                    {
                        itemsToDelete.Add(item2);
                    }
                }
            }
            int collectionSize = collection.Count();
            if (collectionSize > 0)
            {
                AvailableEntityID = collection.ToList().Max((T x) => x.EntityId) + 1;
                List<T> filteredCollection = new List<T>();
                foreach (T item3 in (IEnumerable<T>)collection)
                {
                    if (!itemsToDelete.Any((T i) => i.EntityId == item3.EntityId))
                    {
                        filteredCollection.Add(item3);
                    }
                }
                MaxPages = (int)MathF.Ceiling((float)filteredCollection.Count() / (float)MaxPerPage);
                int startIndex = filteredCollection.Count() - (CurrentPageIndex + 1) * MaxPerPage;
                int endIndex = filteredCollection.Count() - CurrentPageIndex * MaxPerPage;
                startIndex = Math.Clamp(startIndex, 0, filteredCollection.Count());
                endIndex = Math.Clamp(endIndex, 0, filteredCollection.Count());
                if (endIndex > startIndex)
                {
                    ItemList = filteredCollection.Skip(startIndex).Take(endIndex - startIndex).ToList();
                    ItemList.Reverse();
                }
            }
            else
            {
                AvailableEntityID = 1;
                ItemList = new List<T>();
            }
            if (ItemList != null)
            {
                foreach (T item in itemsToDelete)
                {
                    ItemList.Remove(item);
                }
            }
            NewItem = new T();
            return Page();
        }

        public async Task<IActionResult> OnPostAddNew()
        {
            try
            {
                ServiceActionResult dbResult;
                try
                {
                    _context.Set<T>().Add(NewItem);
                    _context.SaveChanges();
                    dbResult = ServiceActionResult.CreateSuccess("Succesfully added to database");
                }
                catch
                {
                    try
                    {
                        NewItem.EntityId = AvailableEntityID;
                        _context.Set<T>().Add(NewItem);
                        _context.SaveChanges();
                    }
                    catch (Exception e2)
                    {
                        return ServiceActionResult.CreateFail("Error during adding item to database: " + e2.Message);
                    }
                    dbResult = ServiceActionResult.CreateSuccess("Succesfully added to database");
                }
                if (dbResult.Result == ServiceActionResult.ActionResult.Success)
                {
                    var contentHandleResult = await ContentEntryHandler.OnContentAdded(NewItem, _context);

                    if(contentHandleResult.Result == ServiceActionResult.ActionResult.Success)
                    {
                        return dbResult;
                    }
                    else
                    {
                        return ServiceActionResult.CreateSuccessWithWarning("");
                    }
                }
                return ServiceActionResult.CreateFail("Error during adding item to database");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return ServiceActionResult.CreateFail("Error during adding item to database: " + e.Message);
            }
        }

        public IActionResult OnPostSaveChanges()
        {
            try
            {
                List<T> collection = _context.Set<T>().ToListAsync().Result;
                foreach (T video in ItemList)
                {
                    _context.Entry(collection.FirstOrDefault((T x) => x.EntityId == video.EntityId)).CurrentValues.SetValues(video);
                }
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return ServiceActionResult.CreateFail("error -> " + JsonSerializer.Serialize(e.ToString()));
            }
            return ServiceActionResult.CreateSuccess("Successfully saved changes");
        }

        public IActionResult OnPostRemoveItem()
        {
            List<T> collection = _context.Set<T>().ToListAsync().Result;
            EntityEntry<T> itemToDelete = _context.Entry(collection.FirstOrDefault((T x) => x.EntityId == DeletedItemID));
            if (((itemToDelete != null) ? itemToDelete.Entity : null) != null)
            {
                try
                {
                    _context.Set<T>().Remove(itemToDelete.Entity);
                    _context.SaveChanges();
                    return ServiceActionResult.CreateSuccess("Removed item succesfully");
                }
                catch (Exception e)
                {
                    return ServiceActionResult.CreateFail("Failed to remove item: " + e.Message);
                }
            }
            return ServiceActionResult.CreateFail("Couldn't find item to delete");
        }

        public IActionResult OnGetCollectFilesFromDirectory(string directory, string filter)
        {
            List<string> fileNames = new List<string>();
            try
            {
                string[] files = Directory.GetFiles(directory);
                if (!string.IsNullOrEmpty(filter))
                {
                    string[] availableTypes = filter.Split(',');
                    for (int j = 0; j < files.Length; j++)
                    {
                        bool isAvailable = false;
                        if (availableTypes.Length != 0)
                        {
                            for (int av = 0; av < availableTypes.Length; av++)
                            {
                                if (Path.GetFileName(files[j]).ToLower().EndsWith(availableTypes[av].ToLower()))
                                {
                                    isAvailable = true;
                                }
                            }
                        }
                        if (isAvailable)
                        {
                            fileNames.Add(Path.GetFileName(files[j]));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        fileNames.Add(Path.GetFileName(files[i]));
                    }
                }
                return ServiceActionResult.CreateSuccess(JsonSerializer.Serialize(fileNames.ToArray()));
            }
            catch (Exception e)
            {
                return ServiceActionResult.CreateFail("Couldn't collect files from directory: " + e.Message);
            }
        }

        public IActionResult OnPostAddNewFile(string directory, string acceptedTypes)
        {
            try
            {
                if (!string.IsNullOrEmpty(directory) && UploadedFile != null)
                {
                    IFormFile uploadedFile = UploadedFile;
                    if (!string.IsNullOrEmpty((uploadedFile != null) ? uploadedFile.FileName : null))
                    {
                        Image imageToSave = GetImageFrom(UploadedFile);
                        if (imageToSave == null && acceptedTypes != null && acceptedTypes.Length != 0)
                        {
                            string[] acceptedTypesArray = acceptedTypes.Split(',');
                            bool wrongFileType = true;
                            for (int i = 0; i < acceptedTypesArray.Length; i++)
                            {
                                if (UploadedFile.FileName.ToLower().EndsWith(acceptedTypesArray[i].ToLower()))
                                {
                                    wrongFileType = false;
                                    break;
                                }
                            }
                            if (wrongFileType)
                            {
                                return ServiceActionResult.CreateFail("Wrong file type: " + UploadedFile.FileName.Split('.').LastOrDefault());
                            }
                        }
                        string localizedName = CompatibilityHelper.ConvertTextToLocalized(UploadedFile.FileName);
                        if (imageToSave != null)
                        {
                            int originalX = imageToSave.Width;
                            int originalY = imageToSave.Height;
                            int aspectRatio = originalX / originalY;
                            int newX = -1;
                            int newY = -1;
                            if (originalX > originalY && originalX > 1024)
                            {
                                newX = 1024;
                                newY = (int)(1024f / (float)originalX * (float)originalY);
                            }
                            else if (originalY > originalX && originalY > 1024)
                            {
                                newY = 1024;
                                newX = (int)(1024f / (float)originalY * (float)originalX);
                            }
                            else if (originalX == originalY && originalX > 1024)
                            {
                                newX = 1024;
                                newY = 1024;
                            }
                            if (newX != -1 && newY != -1)
                            {
                                imageToSave = ResizeImage(imageToSave, newX, newY);
                            }
                            string[] splitLocalName = localizedName.Split('.');
                            if (splitLocalName.Length > 1)
                            {
                                splitLocalName[splitLocalName.Length - 1] = "jpeg";
                                localizedName = string.Join('.', splitLocalName);
                            }
                        }
                        string file = Path.Combine(directory, localizedName);
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        using (FileStream fs = new FileStream(file, FileMode.Create))
                        {
                            if (imageToSave != null)
                            {
                                imageToSave.Save(fs, ImageFormat.Jpeg);
                            }
                            else
                            {
                                UploadedFile.CopyToAsync(fs).Wait();
                            }
                            return ServiceActionResult.CreateSuccess(localizedName);
                        }
                    }
                }
                throw new Exception();
            }
            catch (Exception e)
            {
                return ServiceActionResult.CreateFail("Error during adding a new file: " + e.Message);
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                    return destImage;
                }
            }
        }

        public static Bitmap GetImageFrom(IFormFile file)
        {
            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) && !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            string postedFileExtension = Path.GetExtension(file.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase) && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase) && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase) && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            try
            {
                Bitmap bitmap = new Bitmap(file.OpenReadStream());
                if (bitmap != null)
                {
                    return bitmap;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
