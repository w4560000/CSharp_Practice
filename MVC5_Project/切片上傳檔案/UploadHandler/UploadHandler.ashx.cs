using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Web;

namespace 切片上傳檔案.UploadHandler
{
    /// <summary>
    /// UploadHandler 的摘要描述
    /// </summary>
    public class UploadHandler : IHttpHandler
    {
        private HttpResponse Response;
        private HttpRequest Request;
        private string fileFolderPath = Path.Combine(AppContext.BaseDirectory, "temp");

        public void ProcessRequest(HttpContext context)
        {
            Thread.Sleep(500);
            Request = context.Request;
            Response = context.Response;

            if (Request.Files.Count > 0)
            {
                HttpPostedFile fileUpload = Request.Files[0];
                int totalChunks = Request["totalChunks"] != null ? Convert.ToInt32(Request["totalChunks"]) : 0;
                int currentChunk = Request["currentChunk"] != null ? Convert.ToInt32(Request["currentChunk"]) : 0;
                string fileName = Request["fileName"];

                //判斷是否異常
                if (fileUpload.ContentLength == 0 || totalChunks == 0 || currentChunk == 0 || string.IsNullOrEmpty(fileName))
                {
                    WriteResponse("資料異常");
                    return;
                }

                (bool uploadChunkSuccess, string msg) = this.UploadChunk(fileName, fileUpload.InputStream, currentChunk, fileUpload.ContentLength);

                if (uploadChunkSuccess)
                {
                    if (currentChunk == totalChunks)
                        WriteResponse("上傳完畢", 200);
                    else
                        WriteResponse("片段上傳成功，趕快繼續上傳", 201);
                }
                else
                    WriteResponse(msg);
            }
        }

        private (bool, string) UploadChunk(string uploadedFilename, Stream chunkStream, int currentChunk, int fileLength)
        {
            try
            {
                if (!Directory.Exists(fileFolderPath))
                    Directory.CreateDirectory(fileFolderPath);
            }
            catch(Exception ex)
            {
                return (false, ex.Message);
            }

            string filePath = Path.Combine(fileFolderPath, Path.GetFileName(uploadedFilename));
            Stream stream = null;

            try
            {
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                stream.Seek((currentChunk - 1) * 2 * 1024, SeekOrigin.Begin);
                while ((bytesRead = chunkStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    stream.Write(buffer, 0, bytesRead);
                }
            }
            catch
            {
                return (false, "上傳失敗");
            }
            finally
            {
                if (stream != null)
                    stream.Dispose();
            }

            return (true, "上傳成功");
        }

        private void WriteResponse(string mes, int statusCode = 503)
        {
            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(new { statusCode, mes }));
        }

        public bool IsReusable { get; } = false;
    }
}