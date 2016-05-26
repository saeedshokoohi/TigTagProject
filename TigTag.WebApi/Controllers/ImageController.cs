using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TigTag.DataModel.model;
using TigTag.DTO.ModelDTO.Base;
using TigTag.Repository.ModelRepository;

namespace TigTag.WebApi.Controllers
{
    public class ImageController : BaseController<ImageTable>
    {
        ImageTableRepository imageRepo = new ImageTableRepository();

        public async Task<HttpResponseMessage> UploadImage()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);
            try
            {
                foreach (var file in provider.Contents)
                {
                    if (file.Headers.ContentDisposition.FileName != null)
                    { 
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                        var filetype = file.Headers.ContentType.ToString();
                        var buffer = await file.ReadAsByteArrayAsync();
                        ImageTable image = new ImageTable();
                        image.Id = Guid.NewGuid();
                        image.ImageData = buffer;
                        image.ImageName = filename;
                        image.ImageType = filetype;
                        image.ThumbnailData=createThumbnail(image);
                        imageRepo.Add(image);
                        imageRepo.Save();
                        return Request.CreateResponse(ResultDto.successResult(image.Id.ToString(),"image uploaded successfully..."));
                }
                    //Do whatever you want with filename and its binaray data.
                }
                return Request.CreateResponse(ResultDto.failedResult("no file found..."));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

      
        }

        private byte[] createThumbnail(ImageTable image)
        {
            Stream imagestream = new MemoryStream(image.ImageData);

            IntPtr v = default(IntPtr);
            Image timage=Image.FromStream(imagestream).GetThumbnailImage(100,100,null,v);
            using (var ms = new MemoryStream())
            {
                timage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }
    }
}