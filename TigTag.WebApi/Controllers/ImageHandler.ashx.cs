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
            Guid imageid = Guid.Empty;
            String size = "";
            //getting size of image
            if(context.Request.QueryString["size"]!=null)
            {
                size = context.Request.QueryString["size"];
            }
            //if has pageid
            if (context.Request.QueryString["pageid"] != null)
            {
                string pageidstr = context.Request.QueryString["pageid"];
                Guid pageid = Guid.Empty;
                Guid.TryParse(pageidstr, out pageid);
                if(pageid!=Guid.Empty)
                {
                    PageRepository PageRepo = new PageRepository();
                    Page p=PageRepo.GetSingle(pageid);
                    if (p != null && p.ImageId != null)
                        imageid = (Guid)p.ImageId;
                }

            }
            //if has image id
            else
            if (context.Request.QueryString["id"] != null)
            {
                string idstr = context.Request.QueryString["id"];
                Guid.TryParse(idstr,out imageid);
            }
        //    if (imageid != Guid.Empty)
      //      {
              
                try
                {


                    if (size != null && size == "original")
                    {
                        ImageTable image = imageRepo.GetSingle(imageid);
                        if (image != null)
                        {
                            context.Response.ContentType = image.ImageType;
                            context.Response.BinaryWrite(image.ImageData);
                        }
                        else
                        {
                            throw new Exception("NoImage");
                        }
                    }
                    else
                    {
                        ImageTable image = imageRepo.GetThumbnailImageOnly(imageid);
                        if (image != null)
                        {
                           
                            context.Response.ContentType = image.ImageType;
                            context.Response.BinaryWrite(image.ThumbnailData);
                        }
                        else
                        {
                            throw new Exception("NoImage");
                        }
                    }
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write(ex.Message);
                }
          //  }
            
           
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