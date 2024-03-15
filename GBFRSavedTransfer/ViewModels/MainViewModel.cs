using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GBFRSavedTransfer.Properties;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace GBFRSavedTransfer.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _savedFile1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GBFR", "Saved", "SaveGames", "SaveData1.dat");
    [ObservableProperty]
    private string _savedFile2 = string.Empty;
    [ObservableProperty]
    private DateTime _savedFile1Date = DateTime.MinValue;
    [ObservableProperty]
    private DateTime _savedFile2Date = DateTime.MinValue;
    [ObservableProperty]
    private string _savedFile1Hex = string.Empty;
    [ObservableProperty]
    private string _savedFile2Hex = string.Empty;
    private Visibility file1IsNewer;
    private Visibility file2IsNewer;

    public Visibility File1IsNewer
    {
        get => SavedFile1Date > SavedFile2Date ? Visibility.Visible : Visibility.Collapsed; set => SetProperty(ref file1IsNewer, value);
    }
    public Visibility File2IsNewer
    {
        get => SavedFile2Date > SavedFile1Date ? Visibility.Visible : Visibility.Collapsed; set => SetProperty(ref file2IsNewer, value);
    }

    private string SaveFile1HexBak { get; set; } = string.Empty;
    private string SaveFile2HexBak { get; set; } = string.Empty;

    public MainViewModel()
    {
        SavedFile1Hex = string.Empty;
        SavedFile2Hex = string.Empty;

        if (Application.Current.Properties.Contains(nameof(SavedFile1)))
        {
            InitSaveFile1(Application.Current.Properties[nameof(SavedFile1)].ToString());
        }

        if (Application.Current.Properties.Contains(nameof(SavedFile2)))
        {
            InitSaveFile2(Application.Current.Properties[nameof(SavedFile2)].ToString());
        }
    }

    [RelayCommand]
    private void InitSaveFile1(string filePath)
    {
        SavedFile1 = filePath;
        if (Path.Exists(SavedFile1))
        {
            SavedFile1Date = File.GetLastWriteTime(SavedFile1);
            SavedFile1Hex = ReadTopHexLine(SavedFile1);
            SaveFile1HexBak = SavedFile1Hex;
            Application.Current.Properties[nameof(SavedFile1)] = SavedFile1;
        }
        else
        {
            SavedFile1 = string.Empty;
        }
        OnPropertyChanged(nameof(File1IsNewer));
        OnPropertyChanged(nameof(File2IsNewer));
    }

    [RelayCommand]
    private void InitSaveFile2(string filePath)
    {
        SavedFile2 = filePath;
        if (Path.Exists(SavedFile2))
        {
            SavedFile2Date = File.GetLastWriteTime(SavedFile2);
            SavedFile2Hex = ReadTopHexLine(SavedFile2);
            SaveFile2HexBak = SavedFile2Hex;
            Application.Current.Properties[nameof(SavedFile2)] = SavedFile2;
        }
        else
        {
            SavedFile2 = string.Empty;
        }
        OnPropertyChanged(nameof(File1IsNewer));
        OnPropertyChanged(nameof(File2IsNewer));
    }

    [RelayCommand]
    private void OnOpenFile1()
    {
        OpenFileDialog fileDialog = new()
        {
            Filter = "GBFR Save Files (*.dat)|*.dat",
            Multiselect = false,
            DefaultDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GBFR", "Saved", "SaveGames")
        };
        if (fileDialog.ShowDialog() != true) return;

        if (fileDialog.FileName == SavedFile2) // 1st file cannot be the same as 2nd
        {
            MessageBox.Show("1st file cannot be the same as 2nd", "Error", MessageBoxButton.OK);
            return;
        }

        InitSaveFile1(fileDialog.FileName);
    }

    [RelayCommand]
    private void OnOpenFile2()
    {
        OpenFileDialog fileDialog = new()
        {
            Filter = "GBFR Save Files (*.dat)|*.dat",
            Multiselect = false,
            DefaultDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GBFR", "Saved", "SaveGames")
        };
        if (fileDialog.ShowDialog() != true) return;

        if (fileDialog.FileName == SavedFile1) // 2nd file cannot be the same as 1st
        {
            MessageBox.Show("2nd file cannot be the same as 1st", "Error", MessageBoxButton.OK);
            return;
        }

        InitSaveFile2(fileDialog.FileName);
    }

    [RelayCommand]
    private void OnSave1To2()
    {
        MessageBoxResult r = MessageBox.Show(SavedFile1 + Environment.NewLine + "===>>>" + Environment.NewLine + SavedFile2, Resources.即将覆盖存档并修改文件头, MessageBoxButton.YesNo);
        if (r != MessageBoxResult.Yes) return;

        File.Copy(SavedFile1, SavedFile2, true);
        File.Copy(SavedFile1.Insert(SavedFile1.Length - 4, "_BackUp"), SavedFile2.Insert(SavedFile2.Length - 4, "_BackUp"), true);
        WriteTopHexLine(SavedFile2, SaveFile2HexBak);
        WriteTopHexLine(SavedFile2.Insert(SavedFile2.Length - 4, "_BackUp"), SaveFile2HexBak);
        InitSaveFile2(SavedFile2);
    }

    [RelayCommand]
    private void OnSave2To1()
    {
        MessageBoxResult r = MessageBox.Show(SavedFile2 + Environment.NewLine + "===>>>" + Environment.NewLine + SavedFile1, Resources.即将覆盖存档并修改文件头, MessageBoxButton.YesNo);
        if (r != MessageBoxResult.Yes) return;

        File.Copy(SavedFile2, SavedFile1, true);
        File.Copy(SavedFile2.Insert(SavedFile2.Length - 4, "_BackUp"), SavedFile1.Insert(SavedFile1.Length - 4, "_BackUp"), true);
        WriteTopHexLine(SavedFile1, SaveFile1HexBak);
        WriteTopHexLine(SavedFile1.Insert(SavedFile1.Length - 4, "_BackUp"), SaveFile1HexBak);
        InitSaveFile1(SavedFile1);
    }

    private static string ReadTopHexLine(string filePath)
    {
        int startOffset = 0x00000000; // 起始字节地址
        int length = 0x10; // 要读取的字节数
        byte[] buffer = new byte[length];

        using (FileStream fileStream = File.OpenRead(filePath))
        {
            fileStream.Seek(startOffset, SeekOrigin.Begin);
            fileStream.Read(buffer, 0, length);
        }

        string hex = string.Empty;
        foreach (byte b in buffer)
        {
            hex += $"{b:X2} "; // 以十六进制形式显示每个字节
        }

        return hex.Trim();
    }

    private static void WriteTopHexLine(string filePath, string hex)
    {
        int startOffset = 0x00000000; // 起始字节地址
        string[] hexValues = hex.Split(' '); // 拆分成十六进制值的字符串数组
        byte[] bytes = new byte[hexValues.Length];
        for (int i = 0; i < hexValues.Length; i++)
        {
            bytes[i] = Convert.ToByte(hexValues[i], 16); // 将十六进制字符串转换为字节
        }

        using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Write);
        fileStream.Seek(startOffset, SeekOrigin.Begin);
        fileStream.Write(bytes, 0, bytes.Length);
    }
}
