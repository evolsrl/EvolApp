using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EvolApp.Shared.DTOs;
using EvolApp.Shared.Models;
using EvolAppSocios.Services;
using System.Collections.ObjectModel;

namespace EvolAppSocios.ViewModels;

public partial class VotacionViewModel : ObservableObject
{
    private readonly VotacionApiService _votacionApi;

    public VotacionViewModel(VotacionApiService votacionApi)
    {
        _votacionApi = votacionApi;
    }
}
