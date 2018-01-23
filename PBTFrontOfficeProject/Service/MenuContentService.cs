using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBTFrontOfficeProject.Service
{
    /// <summary>
    /// Altmenülerde yer alan ekranların seçimine göre viewleri çağırır
    /// </summary>
    public class MenuContentService
    {
        public void SetCurrentScreen(IMenuContainer container)
        {
            OnScreenChanged(container);
        }

        public void SetReportsScreen(VReports.EnumView view)
        {
            SetCurrentScreen(new VReports(view));
        }

        public void SetProgramsScreen(VPrograms.EnumView view)
        {
            SetCurrentScreen(new VPrograms(view));
        }

        private void OnScreenChanged(IMenuContainer container)
        {
            if (ScreenChanged != null)
                ScreenChanged(this, container);
        }

        public event EventHandler<IMenuContainer> ScreenChanged;

        ///// <summary>
        ///// Seçili alt ekran işlemleri için kullanılır.
        ///// </summary>
        //EnumContentScreen _CurrentContentScreen;
        //public void SetCurrentScreen(EnumContentScreen contentScreen) {
        //    _CurrentContentScreen = contentScreen;
        //    OnScreenChanged(contentScreen);
        //}

        ///// <summary>
        ///// Değişern ekranları bildirmek için kullanılır.
        ///// </summary>
        ///// <param name="contentScreen">Hangi alt ekranın seçileceğini bildirir.</param>
        //private void OnScreenChanged(EnumContentScreen contentScreen) {
        //    if(ScreenChanged != null)
        //        ScreenChanged(this, contentScreen);
        //}

        //public event EventHandler<EnumContentScreen> ScreenChanged;
    }

    public enum EnumViewGroup
    {
        ReportsView,
        ProgramsView,
    }

    public interface IMenuContainer
    {
         EnumViewGroup ViewGrup { get; }

         int ViewIndex { get; }
    }

    public class VReports : IMenuContainer
    {
        public enum EnumView
        {
            Mali_Hafiza_Raporu = 0,
            Kasa_Giris_Cikis = 1,//Kasa girişi
            Eku_Islemleri = 2,
            Eku_Z_Detay = 3,
            Eku_Tek_Belge_Okuma_Menusu = 4,
            Ekuden_Belge_Okuma_Z_Fis_No = 5,
            Ekuden_Belge_Okuma_Tarih_Saat = 6,
            Ekuden_Belge_Okuma_Tarih_Saat_Fis_No = 7,
            Ekuden_Donemsel_Belge_Okuma = 8,
            Ekuden_Donemsel_Belge_Okuma_Iki_Z_No_Iki_Fis_No = 9,
            Ekuden_Donemsel_Belge_Okuma_Iki_Tarih_Iki_Saat = 10,
            Bos_Ekran=11,
            Kasa_Cikis=12,
        }

        public EnumViewGroup ViewGrup
        {
            get { return EnumViewGroup.ReportsView; }
        }

        public int ViewIndex
        {
            get { return (int)this.View; }
        }

        public EnumView View { get; private set; }

        public VReports(EnumView view)
        {
            this.View = view;
        }
    }

    public class VPrograms : IMenuContainer
    {
        public enum EnumView
        {
            Fis_Basligi_Mesaji_Programlama,
            Fis_Sonu_Mesaji_Programlama,
            KDV_Programlama,
            Odeme_Programlama_Menusu,
            Diger_Odeme_Programlama,
            Doviz_Programlama,
            Bos_Ekran,
            Departman_Programlama,
        }

        public EnumViewGroup ViewGrup
        {
            get { return EnumViewGroup.ProgramsView; }
        }

        public int ViewIndex
        {
            get { return (int)this.View; }
        }

        public EnumView View { get; private set; }

        public VPrograms(EnumView view)
        {
            this.View = view;
        }
    }
}