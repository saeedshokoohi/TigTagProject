using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TigTag.DataModel.model;
using TigTag.Repository.ModelRepository;

namespace TigTag.WebApi.Controllers
{
    /// <summary>
    /// Summary description for ImageHandler
    /// </summary>
    public class ImageHandler : IHttpHandler
    {
        ImageTableRepository imageRepo = new ImageTableRepository();

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.QueryString["id"] != null)
            {
                string idstr = context.Request.QueryString["id"];
                string size= context.Request.QueryString["size"];
                
                try
                {
                    Guid imageid=Guid.Parse(idstr);
                 
                    if (size != null && size == "original")
                    {
                        ImageTable image = imageRepo.GetSingle(imageid);
                        context.Response.ContentType = image.ImageType;
                        context.Response.BinaryWrite(image.ImageData);
                    }
                    else
                    {
                        ImageTable image = imageRepo.GetThumbnailImageOnly(imageid);
                        context.Response.ContentType = image.ImageType;
                        context.Response.BinaryWrite(image.ThumbnailData);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(ex.Message);
                }

            }
           
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}