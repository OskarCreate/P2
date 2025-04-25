using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P2.Data;
using P2.Models;

public class CarritoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public CarritoController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var items = await _context.CarritoItem
            .Where(c => c.UsuarioId == userId)
            .ToListAsync();

        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(int idProducto, int cantidad = 1)
    {
        var userId = _userManager.GetUserId(User);
        var producto = await _context.Producto.FindAsync(idProducto);

        if (producto == null) return NotFound();

        var itemExistente = await _context.CarritoItem
            .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.ProductoId == idProducto);

        if (itemExistente != null)
        {
            itemExistente.Cantidad += cantidad;
        }
        else
        {
            var nuevoItem = new CarritoItem
            {
                UsuarioId = userId,
                ProductoId = producto.Id,
                NombreProducto = producto.Nombre,
                ImagenUrl = producto.ImagenUrl,
                PrecioUnitario = producto.Precio,
                Cantidad = cantidad
            };
            _context.CarritoItem.Add(nuevoItem);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        var item = await _context.CarritoItem.FindAsync(id);
        if (item != null)
        {
            _context.CarritoItem.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> ActualizarCantidad(int id, int cantidad)
    {
        var item = await _context.CarritoItem.FindAsync(id);
        if (item != null && cantidad > 0)
        {
            item.Cantidad = cantidad;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
