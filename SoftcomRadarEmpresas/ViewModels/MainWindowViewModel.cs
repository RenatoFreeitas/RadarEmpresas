using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using SoftcomRadarEmpresas.Models;

namespace SoftcomRadarEmpresas.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private DateTime? _dataInicio = DateTime.Now;
        [ObservableProperty]
        private DateTime? _dataFinal = DateTime.Now;
        [ObservableProperty]
        private string _windowTitle  = "SOFTSHOP > Relatórios > Diversos > Radar Empresa";
        [ObservableProperty]
        private ObservableCollection<string> _listaEmpresas  = new ObservableCollection<string>();
        private ObservableCollection<Empresa> _empresas = new ObservableCollection<Empresa>();
        [ObservableProperty]
        private string _labelEmpresas;
        public ObservableCollection<Empresa> SelecoesAtuais { get; set; }
        public bool IsDropdownOpen { get; set; }
        public string PlaceholderTexto => "Selecione empresas (ex: Matriz, Filial...)";
        
        public MainWindowViewModel(string[] args)
        {
            var matriz = new Empresa
            {
                Nome = "MATRIZ",
                IsSelected = false
            };
            var filialpe = new Empresa
            {
                Nome = "FILIAL PE",
                IsSelected = false
            };
            var filialcg = new Empresa
            {
                Nome = "FILIAL CG",
                IsSelected = false
            };
            _empresas.Add(matriz);
            _empresas.Add(filialpe);
            _empresas.Add(filialcg);

            foreach (var item in _empresas)
            {
                _listaEmpresas.Add(item.Nome);
            }

            SetLabelEmpresas();
            
            SelecoesAtuais = new ObservableCollection<Empresa>();
        }

        public MainWindowViewModel()
        {
            if (Design.IsDesignMode)
            {
                var matriz = new Empresa
                {
                    Nome = "MATRIZ",
                    IsSelected = false
                };
                var filialpe = new Empresa
                {
                    Nome = "FILIAL PE",
                    IsSelected = false
                };
                var filialcg = new Empresa
                {
                    Nome = "FILIAL CG",
                    IsSelected = false
                };
                _empresas.Add(matriz);
                _empresas.Add(filialpe);
                _empresas.Add(filialcg);

                foreach (var item in _empresas)
                {
                    _listaEmpresas.Add(item.Nome);
                }

                SetLabelEmpresas();
            
                SelecoesAtuais = new ObservableCollection<Empresa>();
            }
        }

        public void SetLabelEmpresas()
        {
            LabelEmpresas = $"Empresas ({SelecoesAtuais?.Count ?? 0}/{ListaEmpresas.Count} empresas selecionadas) - ";
        }

        [RelayCommand]
        private void Pesquisar()
        {
            if (SelecoesAtuais?.Count == 0)
            {
                foreach (var item in _empresas)
                {
                    SelecoesAtuais.Add(item);
                    SetLabelEmpresas();
                }
            }
        }
        
    }
}
