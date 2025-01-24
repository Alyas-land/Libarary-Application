using System.ComponentModel.DataAnnotations;
using System.Drawing.Text;

namespace LibararyApplication.Models
{
    public class EditBookViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفاً این فیلد را پر کنید")]
        [MinLength(3, ErrorMessage = "طول نام نامعتبر")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفاً این فیلد را پر کنید")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفاً یک تصویر انتخاب کنید")]
        public int Quantity{ get; set; }

        [Required(ErrorMessage = "لطفاً یک تصویر انتخاب کنید")]
        public IFormFile Picture { get; set; }
    }
}
