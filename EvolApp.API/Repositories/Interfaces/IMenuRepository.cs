﻿using EvolApp.Shared.DTOs;

namespace EvolApp.API.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<IEnumerable<OpcionMenuDto>> GetOpcionesAsync(string documento);
    }
}
