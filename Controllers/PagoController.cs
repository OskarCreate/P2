using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using P2.Data;
using P2.Models;

namespace P2.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public PagoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var carrito = await _context.CarritoItem
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            if (!carrito.Any())
                return RedirectToAction("Index", "Carrito");

            ViewBag.Total = carrito.Sum(i => i.PrecioTotal);
            return View(new Pago());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Pago pago)
        {
            var userId = _userManager.GetUserId(User);
            var carrito = await _context.CarritoItem
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            if (!carrito.Any())
            {
                ModelState.AddModelError("", "Tu carrito está vacío.");
                return View(pago);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Total = carrito.Sum(i => i.PrecioTotal);
                return View(pago);
            }
            pago.UsuarioId = userId;
            pago.PaymentDate = DateTime.Now;
            pago.MontoTotal = carrito.Sum(i => i.PrecioTotal);
            pago.Status = "Procesado";

            _context.Add(pago);
            _context.CarritoItem.RemoveRange(carrito);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmacion");
        }

        public async Task<IActionResult> Confirmacion()
{
    var userId = _userManager.GetUserId(User);

    var ultimoPago = await _context.Pago
        .Where(p => p.UsuarioId == userId)
        .OrderByDescending(p => p.PaymentDate)
        .FirstOrDefaultAsync();

    if (ultimoPago == null)
    {
        return RedirectToAction("Index", "Home");
    }

    return View("Confirmacion", ultimoPago);
}


    }
}
