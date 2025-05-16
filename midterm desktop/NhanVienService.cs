using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace midterm_desktop
{
    public class NhanVien
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public string Email { get; set; }
    }

    public class NhanVienService
    {
        private readonly string filePath;
        public List<NhanVien> DanhSachNhanVien { get; private set; } = new List<NhanVien>();

        public NhanVienService(string filePath)
        {
            this.filePath = filePath;
            LoadFromFile();
        }

        public void LoadFromFile()
        {
            DanhSachNhanVien.Clear();
            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 4)
                    {
                        DanhSachNhanVien.Add(new NhanVien
                        {
                            MaNV = parts[0],
                            HoTen = parts[1],
                            NgaySinh = DateTime.Parse(parts[2]),
                            Email = parts[3]
                        });
                    }
                }
            }
        }

        public void SaveToFile()
        {
            var lines = DanhSachNhanVien.Select(nv =>
                $"{nv.MaNV},{nv.HoTen},{nv.NgaySinh:yyyy-MM-dd},{nv.Email}");
            File.WriteAllLines(filePath, lines);
        }

        public bool MaSoDaTonTai(string maSo)
        {
            return DanhSachNhanVien.Any(nv => nv.MaNV == maSo);
        }

        public void ThemNhanVien(NhanVien nv)
        {
            DanhSachNhanVien.Add(nv);
        }
    }
}

