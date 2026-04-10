using System;
using System.Collections.Generic;
using System.Text;
using Webshop.Application.Interfaces;
using Webshop.Domain.Entitites;

namespace Webshop.Application.Services;

public class KategoriService(IKategoriRepository kategoryRepo) : IKategoriService
{
    public async Task<IEnumerable<Kategori>> GetAllAsync() => 
        await kategoryRepo.GetAllAsync();

    public async Task<Kategori?> GetByIdAsync(Guid id) => 
        await kategoryRepo.GetByIdAsync(id);

    public async Task<Kategori?> GetMostPopularCategoryAsync() => 
        await kategoryRepo.GetMostPopularCategoryAsync();

    public async Task AddAsync(Kategori kategori) =>
        await kategoryRepo.AddAsync(kategori);

    public async Task UpdateAsync(Kategori kategori) => 
        await kategoryRepo.UpdateAsync(kategori);

    public async Task DeleteAsync(Guid id) => 
        await kategoryRepo.DeleteAsync(id);
}