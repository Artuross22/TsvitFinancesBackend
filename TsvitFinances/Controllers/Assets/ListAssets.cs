﻿using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ListAssets : Controller
{
    readonly protected MainDb _mainDb;

    public ListAssets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Asset>>> Index()
    {
        var asset = await _mainDb.Set<Asset>()
            .ToListAsync();

        return asset;
    }
}
