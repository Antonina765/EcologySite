using Ecology.Data.Interface.Repositories;
using Ecology.Data.Models;
using Ecology.Data.Models.Ecology;
using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcologySite.Localizations;
using EcologySite.Models.Ecology;
using EcologySite.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EcologySite.Controllers.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiEcologyController : ControllerBase
    {
        private IEcologyRepositoryReal _ecologyRepository;
        private AuthService _authService;
        private IWebHostEnvironment _webHostEnvironment;
        private IUserRepositryReal _userRepositryReal;
        private ICommentRepositoryReal _commentRepositoryReal;
        
        public ApiEcologyController(IEcologyRepositoryReal ecologyRepository, 
            ICommentRepositoryReal commentRepositoryReal,
            IUserRepositryReal userRepositryReal,
            AuthService authService,
            IWebHostEnvironment webHostEnvironment)
        {
            _ecologyRepository = ecologyRepository;
            _commentRepositoryReal = commentRepositoryReal;
            _userRepositryReal = userRepositryReal;
            _authService = authService;
            _webHostEnvironment = webHostEnvironment;
        }
        
        public bool Remove(int postId)
        {
            var ecology = _ecologyRepository.Get(postId);
            if (ecology != null)
            {
                // Удаление изображения с диска
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, ecology.ImageSrc.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                } 
                _ecologyRepository.Delete(ecology);
            } 
            return true;
        }

        public bool UpdatePost(int id, string url, string text)
        {
            Thread.Sleep(3 * 1000);

            if (CalcCountWorldRepeat.IsEclogyTextHas(text) >= 4)
            {
                ModelState.AddModelError(nameof(PostCreationViewModel.Text), "so similar texts");
                return false;
            }
            _ecologyRepository.UpdatePost(id, url, text);
            return true;
        }
        
        [HttpPost("EcologyChat")]
        public async Task<IActionResult> Create([FromForm] PostCreationViewModel viewModel, IFormFile imageFile)
        {
            if (CalcCountWorldRepeat.IsEclogyTextHas(viewModel.Text) >= 4)
            {
                ModelState.AddModelError(nameof(PostCreationViewModel.Text), "so similar texts");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = _authService.GetUserId();

            string imageUrl = null; //изначально null для того, чтобы затем получить либо URL, либо путь к загруженному изображению с компьютера. Для того, чтобы  использовать одно из значений в объекте EcologyData
            
            if (imageFile != null && imageFile.Length > 0)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;
                var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                var extension = Path.GetExtension(imageFile.FileName);
                var newFileName = $"{fileName}-{currentUserId}{extension}";
                var path = Path.Combine(webRootPath, "images", "uploads", newFileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                imageUrl = $"/images/Ecology/ecologyPosts/{newFileName}";
            }
            
            else if (!string.IsNullOrEmpty(viewModel.Url))
            {
                imageUrl = viewModel.Url;
            }
            
            else
            {
                ModelState.AddModelError("", "Please provide either an image URL or upload an image.");
                return BadRequest(ModelState);
            }

            var ecology = new EcologyData
            {
                ImageSrc = imageUrl,
                Text = viewModel.Text
            };

            _ecologyRepository.Create(ecology, currentUserId!.Value, viewModel.PostId);

            // return RedirectToAction("EcologyChat");
            return Ok();
        }
        
        [Authorize]
        public bool Like(int ecologyId)
        {
            var userId = _authService.GetUserId()!.Value;

            return _ecologyRepository.LikeEcology(ecologyId, userId);
        }
    }
}
