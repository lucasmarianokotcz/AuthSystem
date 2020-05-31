using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AuthSystem.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class AlterarSenhaModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AlterarSenhaModel> _logger;

        public AlterarSenhaModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AlterarSenhaModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [DataType(DataType.Password)]
            [Display(Name = "Senha atual")]
            public string Password { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [StringLength(20, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nova senha")]
            public string NewPassword { get; set; }

            [Required(ErrorMessage = "Confirme a senha.")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirme a senha")]
            [Compare("NewPassword", ErrorMessage = "A senha e a confirmação de senha não combinam.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
            {
                return Page();
            }            

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com ID '{_userManager.GetUserId(User)}'.");
            }            

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.Password, Input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
            else
            {
                if (Input.Password == Input.NewPassword)
                {
                    // rollback senha
                    await _userManager.ChangePasswordAsync(user, Input.NewPassword, Input.Password);
                    ModelState.AddModelError(string.Empty, "A nova senha não pode ser igual a senha atual.");
                    return Page();
                }

            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Alert"] = "Senha alterada com sucesso!";
            _logger.LogInformation("Usuário alterou a senha com sucesso.");

            return LocalRedirect(returnUrl);
        }
    }
}
