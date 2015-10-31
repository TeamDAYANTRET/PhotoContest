namespace PhotoContest.Web.Attributes
{
    using System.Web;
    using System.ComponentModel.DataAnnotations;    

    public class ValidImage : ValidationAttribute
    {
        public long MaxSizeInBytes { get; set; }

        public ValidImage(long maxSizeInBytes)
        {
            this.MaxSizeInBytes = maxSizeInBytes;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return null;
            }

            HttpPostedFileBase image = value as HttpPostedFileBase;
            System.Drawing.Image imgObj = System.Drawing.Image.FromStream(image.InputStream);

            if (
                   (imgObj.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Icon.Guid) ||
                   (imgObj.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Bmp.Guid)
               )
            {
                return new ValidationResult("Unsupported image format. This site accepts only JPEG, JPG, GIF and PNG images.");
            } else if 
               (
                   !(
                       (imgObj.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Jpeg.Guid) ||
                       (imgObj.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Gif.Guid) ||
                       (imgObj.RawFormat.Guid == System.Drawing.Imaging.ImageFormat.Png.Guid)
                   )
               )
                return new ValidationResult("Selected file is not a valid image.");

            if ((long)(image.ContentLength) > this.MaxSizeInBytes)
            {
                var maxSizeInMB = this.MaxSizeInBytes / 1024 / 1024;
                return new ValidationResult(string.Format("Max allowed size is: {0:F2} MB", maxSizeInMB));
            }

            return null;
        }
    }
}
