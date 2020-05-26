using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AuthSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
namespace AuthSystem.Areas.Conta.Pages
{
    [Authorize]
    public class GerenciarModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<GerenciarModel> _logger;
        private readonly IEmailSender _emailSender;

        public GerenciarModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<GerenciarModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Nome")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Sobrenome")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [EmailAddress(ErrorMessage = "O campo {0} não é um endereço de e-mail válido.")]
            [Display(Name = "E-mail")]
            public string Email { get; set; }

            //[Required(ErrorMessage = "O campo {0} é obrigatório.")]
            //[StringLength(20, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            //[DataType(DataType.Password)]
            //[Display(Name = "Senha")]
            //public string Password { get; set; }

            //[Required(ErrorMessage = "Confirme a senha.")]
            //[DataType(DataType.Password)]
            //[Display(Name = "Confirme a senha")]
            //[Compare("Password", ErrorMessage = "A senha e a confirmação de senha não combinam.")]
            //public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                user.UserName = Input.Email;
                user.Email = Input.Email;
                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Alert"] = "Usuário atualizado com sucesso!";
                    _logger.LogInformation("Usuário atualizado com sucesso.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
