using Avalonia.Controls;
using System;
using System.Linq;
using Avalonia.Input;
using Avalonia.Interactivity;
using SoftcomRadarEmpresas.Models;
using SoftcomRadarEmpresas.ViewModels;

namespace SoftcomRadarEmpresas.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        private void PeriodoChanged(object sender, EventArgs eventArgs)
        {
            var hoje = DateTime.Today;

            switch (CboPeriodo?.SelectedIndex)
            {
                case 0: // Hoje
                    TxtDataInicial.SelectedDate = hoje;
                    TxtDataFinal.SelectedDate = hoje;
                    break;
                case 1: // Ontem
                    TxtDataInicial.SelectedDate = hoje.AddDays(-1);
                    TxtDataFinal.SelectedDate = hoje.AddDays(-1);
                    break;
                case 2: // �ltimos 7 dias
                    TxtDataInicial.SelectedDate = hoje.AddDays(-6);
                    TxtDataFinal.SelectedDate = hoje;
                    break;
                case 3: // �ltimos 30 dias
                    TxtDataInicial.SelectedDate = hoje.AddDays(-29);
                    TxtDataFinal.SelectedDate = hoje;
                    break;
                case 4: // M�s Atual
                    TxtDataInicial.SelectedDate = new DateTime(hoje.Year, hoje.Month, 1);
                    TxtDataFinal.SelectedDate = TxtDataInicial.SelectedDate.Value.AddMonths(1).AddDays(-1);
                    break;
                case 5: // �ltimos 3 meses
                    TxtDataInicial.SelectedDate = hoje.AddMonths(-3).AddDays(1);
                    TxtDataFinal.SelectedDate = hoje;
                    break;
                case 6: // �ltimos 6 meses
                    TxtDataInicial.SelectedDate = hoje.AddMonths(-6).AddDays(1);
                    TxtDataFinal.SelectedDate = hoje;
                    break;
                case 7: // Ano
                    TxtDataInicial.SelectedDate = new DateTime(hoje.Year, 1, 1);
                    TxtDataFinal.SelectedDate = new DateTime(hoje.Year, 12, 31);
                    break;
            }
        }
        private void InputContainer_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                // Apenas alterne a visibilidade se o clique não for para remover uma tag
                if (e.Source is Button removeButton && removeButton.Classes.Contains("tag-remove"))
                {
                    // O comando já lida com a remoção, não faça mais nada.
                }
                else
                {
                    vm.IsDropdownOpen = !vm.IsDropdownOpen;
                }
                
                PopEmpresas.IsOpen = vm.IsDropdownOpen;
            }
        }
        
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                
                // Adicionar itens selecionados à lista de SelecoesAtuais
                foreach (var item in e.AddedItems)
                {
                    if (item == null)
                        return;

                    var empresa = new Empresa()
                    {
                        Nome = item.ToString(),
                        IsSelected = true
                    };
                    
                    bool jaExiste = vm.SelecoesAtuais.Any(x => x.Nome == item.ToString());
                    
                    if (!jaExiste)
                    {
                        vm.SelecoesAtuais.Add(empresa);
                    }
                }
                
                // Remover itens desmarcados da lista de SelecoesAtuais
                foreach ( var item in e.RemovedItems)
                {
                    var empresaDesmarcar = new Empresa()
                    {
                        Nome = item.ToString(),
                    };
                    vm.SelecoesAtuais.Remove(empresaDesmarcar);
                }
                
                LbxEmpresas.SelectedItems = null;
                vm.SetLabelEmpresas();
                vm.IsDropdownOpen = false;
                PopEmpresas.IsOpen = vm.IsDropdownOpen;
            }
        }

        private void RemoverEmpresa(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                if (sender is Button button)
                {
                    if(button.DataContext is Empresa empresaRemover)
                    {
                        vm.SelecoesAtuais.Remove(empresaRemover);
                    }
                }
                vm.SetLabelEmpresas();
            }
        }
        
        private void RemoverTodasEmpresas(object sender, PointerPressedEventArgs e)
        {
            var propetiers = e.GetCurrentPoint(this).Properties;
            if (propetiers.IsLeftButtonPressed)
            {
                if (DataContext is MainWindowViewModel vm)
                {
                    vm.SelecoesAtuais.Clear();
                    vm.SetLabelEmpresas();
                }
            }
        }
    }
}