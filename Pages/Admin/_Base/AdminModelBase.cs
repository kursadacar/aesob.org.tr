using Microsoft.AspNetCore.Mvc;
using aesob.org.tr.Models;
using aesob.org.tr.Pages.Content.Base;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using aesob.org.tr.Utilities;
using Microsoft.AspNetCore.Hosting;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace aesob.org.tr.Pages.Admin
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

    public abstract class AdminModelBase<T> : AesobModelBase, IAdminModel, INavigablePage where T: class, IAesobEntity, new()
    {
        public int MaxPerPage => 16;
        public string ContentPageName => "Admin/" + GetType().Name.Replace("Model", "");
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

        object IAdminModel.NewItem => NewItem;

        private List<object> _itemList;
        List<object> IAdminModel.ItemList
        {
            get
            {
                if(_itemList == null && ItemList != null)
                {
                    _itemList = new List<object>();
                    foreach(var item in ItemList)
                    {
                        _itemList.Add(item);
                    }
                }
                return _itemList;
            }
        }

        public List<string> FilesInDirectory { get; private set; }

        public string ContentParameterName => "pageIndex";

        public string CurrentQuery { get; private set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public AdminModelBase(AesobDbContext context) : base(context)
        {

        }

        public IActionResult OnGet(string query = null, int pageIndex = 0)
        {
            if (!User.Identity.IsAuthenticated)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return new RedirectToPageResult("/Index");
            }

            CurrentQuery = query;
            CurrentPageIndex = pageIndex;
            var collection = _context.Set<T>();

            List<T> itemsToDelete = new List<T>();

            //TODO_Kursad: Optimize
            if (!string.IsNullOrEmpty(query))
            {
                var itemsCopy = collection.ToList();

                foreach (var item in itemsCopy)
                {
                    bool isItemFiltered = true;
                    foreach (var property in item.GetType().GetProperties())
                    {
                        var propValue = property.GetValue(item);

                        if (propValue?.ToString().ToLower().Contains(query.ToLower()) == true)
                        {
                            isItemFiltered = false;
                        }
                    }
                    if (isItemFiltered)
                    {
                        itemsToDelete.Add(item);
                    }
                }
            }

            int collectionSize = collection.Count();

            if (collectionSize > 0)
            {
                AvailableEntityID = collection.ToList().Max(x => x.EntityId) + 1;

                var filteredCollection = new List<T>();
                foreach (var item in collection)
                {
                    if (!itemsToDelete.Any(i => i.EntityId == item.EntityId))
                    {
                        filteredCollection.Add(item);
                    }
                }

                MaxPages = (int)MathF.Ceiling((float)filteredCollection.Count() / (float)MaxPerPage);

                int startIndex = filteredCollection.Count() - ((CurrentPageIndex + 1) * MaxPerPage);
                int endIndex = filteredCollection.Count() - (CurrentPageIndex * MaxPerPage);

                startIndex = Math.Clamp(startIndex, 0, filteredCollection.Count());
                endIndex = Math.Clamp(endIndex, 0, filteredCollection.Count());

                if(endIndex > startIndex)
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

            if(ItemList != null)
            {
                foreach (var item in itemsToDelete)
                {
                    ItemList.Remove(item);
                }
            }

            NewItem = new T();

            return Page();
        }

        public IActionResult OnPostAddNew()
        {
            try
            {
                try
                {
                    _context.Set<T>().Add(NewItem);
                    _context.SaveChanges();
                }
                catch
                {
                    NewItem.EntityId = AvailableEntityID;
                    _context.Set<T>().Add(NewItem);
                    _context.SaveChanges();
                }
                return new ObjectResult("success");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ObjectResult("error");
            }
        }

        public IActionResult OnPostSaveChanges()
        {
            try
            {
                var collection = _context.Set<T>().ToListAsync().Result;

                foreach (var video in ItemList)
                {
                    _context.Entry(collection.FirstOrDefault(x => x.EntityId == video.EntityId)).CurrentValues.SetValues(video);
                }

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return new ObjectResult("error -> " + JsonSerializer.Serialize(e.ToString()));
            }
            return new ObjectResult("success");
        }

        public IActionResult OnPostRemoveItem()
        {
            var collection = _context.Set<T>().ToListAsync().Result;

            var itemToDelete = _context.Entry(collection.FirstOrDefault(x => x.EntityId == DeletedItemID));
            if(itemToDelete?.Entity != null)
            {
                _context.Set<T>().Remove(itemToDelete.Entity);
                _context.SaveChanges();
                return new ObjectResult("success");
            }
            else
            {
                return new ObjectResult("error");
            }
        }

        public IActionResult OnGetCollectFilesFromDirectory(string directory, string filter)
        {
            try
            {
                var files = Directory.GetFiles(directory);
                List<string> fileNames = new List<string>();

                if (!string.IsNullOrEmpty(filter))
                {
                    string[] availableTypes = filter.Split(',');

                    for (int i = 0; i < files.Length; i++)
                    {
                        bool isAvailable = false;
                        if (availableTypes.Length > 0)
                        {
                            for (int av = 0; av < availableTypes.Length; av++)
                            {
                                if (Path.GetFileName(files[i]).ToLower().EndsWith(availableTypes[av].ToLower()))
                                {
                                    isAvailable = true;
                                }
                            }
                        }
                        if (isAvailable)
                        {
                            fileNames.Add(Path.GetFileName(files[i]));
                        }
                    }
                }
                else
                {
                    for(int i = 0; i < files.Length; i++)
                    {
                        fileNames.Add(Path.GetFileName(files[i]));
                    }
                }

                return new ObjectResult(JsonSerializer.Serialize(fileNames.ToArray()));
            }
            catch
            {
                return new ObjectResult("error");
            }
        }

        public IActionResult OnPostAddNewFile(string directory, string acceptedTypes)
        {
            try
            {
                if(string.IsNullOrEmpty(directory) || UploadedFile == null || string.IsNullOrEmpty(UploadedFile?.FileName))
                {
                    throw new Exception();
                }

                if(acceptedTypes != null && acceptedTypes.Length != 0)
                {
                    var acceptedTypesArray = acceptedTypes.Split(',');

                    bool wrongFileType = true;
                    for (int i = 0; i < acceptedTypesArray.Length; i++)
                    {
                        if (UploadedFile.FileName.ToLower().EndsWith(acceptedTypesArray[i].ToLower()))
                        {
                            wrongFileType = false;
                        }
                    }
                    if (wrongFileType)
                    {
                        return new ObjectResult("wrong-type");
                    }
                }

                var localizedName = CompatibilityHelper.ConvertTextToLocalized(UploadedFile.FileName);

                Image imageToSave = GetImageFrom(UploadedFile);

                if(imageToSave != null)
                {
                    var originalX = imageToSave.Width;
                    var originalY = imageToSave.Height;

                    var aspectRatio = originalX / originalY;

                    int newX = -1;
                    int newY = -1;

                    if (originalX > originalY && originalX > 1024)
                    {
                        newX = 1024;
                        newY = (int)(1024f / originalX * originalY);
                    }
                    else if(originalY > originalX && originalY > 1024)
                    {
                        newY = 1024;
                        newX = (int)(1024f / originalY * originalX);
                    }
                    else if(originalX == originalY && originalX > 1024)
                    {
                        newX = 1024;
                        newY = 1024;
                    }

                    if(newX != -1 && newY != -1)
                    {
                        imageToSave = ResizeImage(imageToSave, newX, newY);
                    }

                    var splitLocalName = localizedName.Split('.');
                    if (splitLocalName.Length > 1)
                    {
                        splitLocalName[splitLocalName.Length - 1] = "jpeg";

                        localizedName = string.Join('.', splitLocalName);
                    }
                }

                var file = Path.Combine(directory, localizedName);
                using (FileStream fs = new FileStream(file, FileMode.Create))
                {
                    if(imageToSave != null)
                    {
                        imageToSave.Save(fs, ImageFormat.Jpeg);
                    }
                    else
                    {
                        UploadedFile.CopyToAsync(fs).Wait();
                    }
                    return new ObjectResult(localizedName);
                }
            }
            catch (Exception e)
            {
                return new ObjectResult("error" + e.ToString());
            }
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap GetImageFrom(IFormFile file)
        {
            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var postedFileExtension = Path.GetExtension(file.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            try
            {
                var bitmap = new Bitmap(file.OpenReadStream());
                if(bitmap != null)
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
