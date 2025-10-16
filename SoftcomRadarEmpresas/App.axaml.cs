using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using SoftcomRadarEmpresas.ViewModels;
using SoftcomRadarEmpresas.Views;

namespace SoftcomRadarEmpresas;

public class App : Application
{
    public string[] Args { get; private set; }
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Args = desktop.Args;
            
#if DEBUG
            if (Args?.Length == 0)
            {
                string debugArg = @"{'DataBaseType':'MSSQL',
                                    'DataBaseServer':'localhost\\SQLEXPRESS',
                                    'DataBaseUser':'sa',
                                    'DataBasePassword':'qaz@123',
                                    'DataBasePortNumber':5433,
                                    'DataBaseTimeout':'120',
                                    'DataBaseName':'BaseDeliteImports',
                                    'ModuloApp':'PADRAO'}";

                string[] argsDebug = new string[] { debugArg };
                Args = argsDebug;
            }
#endif
            
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(Args),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}