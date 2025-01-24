using System.ComponentModel.DataAnnotations;

namespace LibararyApplication.Models
{
    public class AddBookViewModel
    {
        [Required(ErrorMessage = "لطفاً این فیلد را پر کنید")]
    
        [MinLength(3, ErrorMessage = "طول نام نامعتبر")]
        public string Name { get; set; }

        [Required(ErrorMessage = "لطفاً این فیلد را پر کنید")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفاً این فیلد را پر کنید")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "لطفاً این فیلد را پر کنید")]
        public int PagePaper { get; set; }

        [Required(ErrorMessage = "لطفاً یک تصویر انتخاب کنید")]
        public IFormFile Picture { get; set; }

        public int CategoryId { get; set; }
    }



}
