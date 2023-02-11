using FileManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FileManagerApp.Controllers
{
    public class FileManagementController : Controller
    {
        string rootPath = "D:\\YORE\\INSIDE DATA";
        public IActionResult BaseRoot()
        {
            string urlPath = TempData["urlPath"]?.ToString();
            var directoryName = DirectoriesList(urlPath);
            return View(directoryName);
        }

        public IActionResult SubRootView(string folderName, string URL)
        {
            var files = SubDirectoriesList(folderName, URL);
            return View(files);
        }

        private List<FileModels> DirectoriesList(string urlpath)
        {
            if (urlpath != null)
            {
                string url = this.HttpContext.Request.Path.Value.ToString();
                if (url != "/directories/Index")
                {
                    var folderName = Directory.GetDirectories(urlpath, "*", SearchOption.TopDirectoryOnly);
                    List<FileModels> importList = new List<FileModels>();

                    foreach (var folders in folderName)
                    {
                        importList.Add(new FileModels
                        {
                            folderName = Path.GetFileName(folders),
                            fullpath = Path.GetFullPath(folders),
                            backPath = Path.GetFullPath(folders),
                        });
                    }
                    return importList;
                }
                else
                {
                    var folderName = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
                    List<FileModels> importList = new List<FileModels>();

                    var filesName = Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly)
                  .Where(s => s.EndsWith(".pdf") || s.EndsWith(".xlsx") || s.EndsWith(".docx"));

                    foreach (var folders in folderName)
                    {
                        importList.Add(new FileModels
                        {
                            folderName = Path.GetFileName(folders),
                            fullpath = Path.GetFullPath(folders),

                        });
                    }

                    foreach (var files in filesName)
                    {
                        importList.Add(new FileModels
                        {
                            fileName = Path.GetFileNameWithoutExtension(files),
                            extension = Path.GetExtension(files),
                            folderName = Path.GetDirectoryName(files),
                            fullFileName = Path.GetFullPath(files),

                        });
                    }
                    return importList;
                }
            }
            else
            {
                var folderName = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
                List<FileModels> importList = new List<FileModels>();

                var filesName = Directory.GetFiles(rootPath, "*.*", SearchOption.TopDirectoryOnly)
              .Where(s => s.EndsWith(".pdf") || s.EndsWith(".xlsx") || s.EndsWith(".docx"));

                foreach (var folders in folderName)
                {
                    importList.Add(new FileModels
                    {
                        folderName = Path.GetFileName(folders),
                        fullpath = Path.GetFullPath(folders),

                    });
                }

                foreach (var files in filesName)
                {
                    importList.Add(new FileModels
                    {
                        fileName = Path.GetFileNameWithoutExtension(files),
                        extension = Path.GetExtension(files),
                        folderName = Path.GetDirectoryName(files),
                        fullFileName = Path.GetFullPath(files),

                    });
                }

                return importList;
            }
        }


        public List<FileModels> SubDirectoriesList(string folderName, string URL)
        {
            TempData["urlPath"] = folderName;
            var currentPath = "";
            if (folderName == URL)
            {
                URL = "";
                currentPath = Path.Combine(rootPath, folderName);
            }
            else if (folderName != URL)
            {
                currentPath = URL + @"\" + folderName;
            }
            var directoryInfo = new DirectoryInfo(currentPath);
            if (!string.IsNullOrEmpty(currentPath))
            {
                var folders = DirectoriesList(currentPath);
                List<FileModels> list = new List<FileModels>();
                ViewBag.currentTitle = directoryInfo.Name;

                foreach (var subDirectoryName in folders)
                {
                    list.Add(new FileModels
                    {
                        subDirName = Path.GetFileName(subDirectoryName.folderName),
                        fullpath = Path.GetFullPath(subDirectoryName.fullpath),
                        currentParent = currentPath,
                        backPath = subDirectoryName.backPath.Split(@"\\").LastOrDefault(),
                        attribute = directoryInfo.Attributes.ToString(),
                    });
                }

                var filesName = Directory.GetFiles(currentPath, "*.*", SearchOption.TopDirectoryOnly)
               .Where(s => s.EndsWith(".pdf") || s.EndsWith(".xlsx") || s.EndsWith(".docx"));


                foreach (var files in filesName)
                {
                    list.Add(new FileModels
                    {
                        fileName = Path.GetFileNameWithoutExtension(files),
                        extension = Path.GetExtension(files),
                        folderName = Path.GetDirectoryName(files),
                        fullFileName = Path.GetFullPath(files),

                    });
                }
                return list;
            }
            else
            {
                return null;
            }
        }
    }
}
