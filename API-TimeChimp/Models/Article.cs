using System.Text.RegularExpressions;

namespace Api.Devion.Models;

public class ArticleETS
{
    public string? ART_NR { get; set; }
    public string? ART_LEV1 { get; set; }
    public string? ART_LEV2 { get; set; }
    public string? ART_LEV3 { get; set; }
    public string? ART_LEV4 { get; set; }
    public string? ART_OMS { get; set; }
    public float? ART_AANKP { get; set; }
    public string? ART_VERKP { get; set; }
    public string? ART_INVPR { get; set; }
    public string? ART_BTW { get; set; }
    public string? ART_EENH { get; set; }
    public string? ART_MUTATI { get; set; }
    public string? ART_MINST { get; set; }
    public string? ART_MAXST { get; set; }
    public string? ART_STOCK { get; set; }
    public string? ART_BACKOR { get; set; }
    public string? ART_GERES { get; set; }
    public string? ART_BHOEV { get; set; }
    public string? ART_AANKPV { get; set; }
    public string? ART_MNT { get; set; }
    public string? ART_INFO1 { get; set; }
    public string? ART_INFO2 { get; set; }
    public string? ART_INFO3 { get; set; }
    public string? ART_INFO4 { get; set; }
    public string? ART_INFO5 { get; set; }
    public string? ART_INFO6 { get; set; }
    public string? ART_PR1 { get; set; }
    public string? ART_PR2 { get; set; }
    public string? ART_PR3 { get; set; }
    public string? ART_PR4 { get; set; }
    public string? ART_DAT1 { get; set; }
    public string? ART_DAT2 { get; set; }
    public string? ART_DAT3 { get; set; }
    public string? ART_DAT4 { get; set; }
    public string? ART_AAN1 { get; set; }
    public string? ART_AAN2 { get; set; }
    public string? ART_AAN3 { get; set; }
    public string? ART_AAN4 { get; set; }
    public string? ART_FAM { get; set; }
    public string? ART_ACTIEF { get; set; }
    public string? ART_STOCK2 { get; set; }
    public string? ART_STOCK3 { get; set; }
    public string? ART_STOCK4 { get; set; }
    public string? ART_MAT { get; set; }
    public string? ART_FASE1 { get; set; }
    public string? ART_FASE2 { get; set; }
    public string? ART_BARC { get; set; }
    public string? ART_OLEV { get; set; }
    public string? ART_VERKP2 { get; set; }
    public string? ART_IMP { get; set; }
    public string? ART_MERK { get; set; }
    public string? ART_VELD1 { get; set; }
    public string? ART_VELD2 { get; set; }
    public string? ART_OMSENG { get; set; }
    public string? ART_OMSDUI { get; set; }
    public string? ART_OMSFRA { get; set; }
    public string? ART_VERBTW { get; set; }
    public string? ART_MEMO1 { get; set; }
    public string? ART_NEWSTC { get; set; }
    public string? ART_REMARK { get; set; }
    public string? ART_INVAAN { get; set; }
    public string? ART_WPROC { get; set; }
    public string? ART_WVERK { get; set; }
    public string? ART_LNDCDE { get; set; }
    public string? ART_BARNR { get; set; }
    public string? ART_AFRONDEN { get; set; }
    public string? ART_AFAANTAL { get; set; }
    public string? ART_LEVREF { get; set; }
    public string? ART_INTRASTAT { get; set; }
    public string? ART_PALLET { get; set; }
    public string? ART_CAT { get; set; }
    public string? ART_KORT { get; set; }
    public string? ART_TOEB { get; set; }
    public string? ART_MONT { get; set; }
    public string? ART_TOESL { get; set; }
    public string? ART_MATERIAAL { get; set; }
    public string? ART_INHOUD { get; set; }
    public string? ART_DIAMETER { get; set; }
    public string? ART_LENGTE { get; set; }
    public string? ART_BREEDTE { get; set; }
    public string? ART_WANDDIKTE { get; set; }
    public string? ART_PLAATS { get; set; }
    public string? ART_DRUK { get; set; }
    public string? ART_HOOGTE { get; set; }
    public string? ART_VH { get; set; }
    public string? ART_SOORT { get; set; }
    public string? ART_SGEW { get; set; }
    public string? ART_KGM { get; set; }
    public string? ART_OLDSTOCK { get; set; }
    public string? ART_NR_GRID { get; set; }
    public string? ART_GERESERVEERD { get; set; }
    public string? ART_PORTEFEUILLE { get; set; }
    public string? ART_REK { get; set; }
    public string? ART_GEWICHT { get; set; }
    public string? ART_BESTEL { get; set; }
    public string? ART_SELECT { get; set; }
    public string? ART_BESTELLEV { get; set; }
    public string? ART_IMP_AANK { get; set; }
    public string? ART_RESSTOCK { get; set; }
    public string? ART_VOLUME { get; set; }
    public string? ART_NETTOGEWICHT { get; set; }
    public string? ART_DIAVOET { get; set; }
    public string? ART_DIATOP { get; set; }
    public string? ART_LPH { get; set; }
    public string? ART_LUITNR { get; set; }
    public string? ART_HOEKUITH { get; set; }
    public string? ART_TYPEMAT { get; set; }
    public string? ART_TEKENING { get; set; }
    public string? ART_OPPBEH1 { get; set; }
    public string? ART_OPPBEH2 { get; set; }
    public string? ART_OMSNED2 { get; set; }
    public string? ART_OMSFR2 { get; set; }
    public string? ART_OMSD2 { get; set; }
    public string? ART_AKTIEF { get; set; }
    public string? ART_OMSENG2 { get; set; }
    public string? ART_LEVTERM { get; set; }
    public string? ART_AFM1 { get; set; }
    public string? ART_AFM2 { get; set; }
    public string? ART_AFM3 { get; set; }
    public string? ART_NR2 { get; set; }
    public string? ART_CATEGORIE { get; set; }
    public string? ART_GROEP { get; set; }
    public string? ART_SUBGROEP { get; set; }
    public string? ART_KORTINGSGROEP { get; set; }
    public string? ART_VERPAKEENH { get; set; }
    public string? BESTHOEV { get; set; }
    public string? LEVTERM { get; set; }
    public string? LEVEENH { get; set; }
    public string? GARANTIE { get; set; }
    public string? ART_KOMMISSIE { get; set; }
    public string? ART_STANDAARD { get; set; }
    public string? ART_TIJD_UITV { get; set; }
    public string? ART_TIJD_OPVOLG { get; set; }
    public string? ART_PRIJSWIJZIGING { get; set; }
    public string? ART_GEMP { get; set; }
    public string? ART_OPP_PER_METER { get; set; }
    public string? ART_GEENSTDKORTING { get; set; }
    public string? ART_UPDATE { get; set; }
    public string? ART_CONVERSIEFACTOR_TYPE { get; set; }
    public string? ART_BEREKENINGSWIJZE { get; set; }
    public string? ART_VERPAKT_PER { get; set; }
    public string? ART_CONVERSIEFACTOR { get; set; }
    public string? ART_SUBFAM { get; set; }
    public string? ART_VERK_EENH { get; set; }
    public string? ART_REMARKDUI { get; set; }
    public string? ART_REMARKENG { get; set; }
    public string? ART_REMARKFRA { get; set; }
    public string? ART_FICTIEVE_AANKP { get; set; }
    public string? ART_VRIJ1 { get; set; }
    public string? ART_SEL { get; set; }
    public string? ART_BESTELPROJ { get; set; }
    public string? ART_BESTELSUBPROJ { get; set; }
    public string? ART_VASTEKOST { get; set; }
    public string? ART_SAMENGESTELD { get; set; }
    public string? ART_ONDERLIGGEND { get; set; }
    public string? ART_KLEUR { get; set; }
    public string? ART_ID { get; set; }
    public string? ART_USER { get; set; }
    public string? ART_BESTEL_LEN { get; set; }
    public string? ART_BESTEL_DIK { get; set; }
    public string? ART_BESTEL_HOOG { get; set; }
    public string? ART_BESTELAANKP { get; set; }
    public string? ART_AANTAL { get; set; }
    public string? ART_BACKORDER_KLANT { get; set; }
    public string? ART_BACKORDER_LEVER { get; set; }
    public string? ART_SYSDATUM { get; set; }
    public string? ART_REMARK_INTERN { get; set; }
    public string? ART_REMARK_AANKOOP { get; set; }
    public string? ART_REMARK_AANKOOP_FRA { get; set; }
    public string? ART_REMARK_AANKOOP_ENG { get; set; }
    public string? ART_REMARK_AANKOOP_DUI { get; set; }
    public string? ART_BARCODE_AFGEDRUKT { get; set; }
    public string? ART_FOTO_EXISTS { get; set; }
    public string? ART_WBEDRAG { get; set; }
    public string? ART_AANKP_VERK { get; set; }
    public string? ART_VERKP_VERK { get; set; }
    public string? ART_PROGRESS_RECID { get; set; }
    public string? DATE_CHANGED { get; set; }
    public string? ART_STATUS_BESTELAANVRAAG { get; set; }
    public string? ART_TEKENINGNUMMER { get; set; }
    public string? ART_VERPAKKINGSARTIKEL { get; set; }
    public string? ART_KOPPELING_DERDEN { get; set; }
    public string? ART_GEKOPPELD_STUKNUMMER { get; set; }
    public string? ART_GESCAND_STUKS { get; set; }
    public string? ART_MEEREKENEN_TOTAAL { get; set; }
    public string? ART_NR_UPPER { get; set; }
    public string? ART_DATUM_PRIJSWIJZIGING { get; set; }
    public string? ART_STOCK_STUK { get; set; }
    public string? ART_STOCK_EHV { get; set; }
    public string? ART_UPDATE_KARDEX { get; set; }
    public string? ART_STOCK_OPMERKING { get; set; }
    public string? ART_LAATSTE_AANKOOPDATUM { get; set; }
    public string? ART_BACKORDER_KLANT_STUK { get; set; }
    public string? ART_BACKORDER_LEVER_STUK { get; set; }
    public string? ART_WIJZIGING_CONF { get; set; }
    public string? ART_PRODUCEER { get; set; }
    public string? ART_AANKOOP_PER { get; set; }
    public string? ART_BESTELARTIKEL { get; set; }
    public string? ART_HYPERLINK { get; set; }
    public string? FIRST_USER { get; set; }
    public string? LAST_USER { get; set; }
    public string? CREATION { get; set; }
    public string? ART_SUBFAMILIE_DETAIL_ID { get; set; }
    public string? ART_OMS_UPPER { get; set; }
    public string? ART_SOORT_OMS { get; set; }
    public string? ART_ARTIKELGROEP_ID { get; set; }
    public string? ART_LOTNUMMER { get; set; }
    public string? ART_MINSTOCK_IN { get; set; }
    public string? ART_MAXSTOCK_IN { get; set; }
    public string? ART_ADMIN2MOBILE { get; set; }
    public string? ART_AANTAL_IN_PRODUCTIE { get; set; }
    public string? ART_FICTIEVE_STOCK { get; set; }
    public string? ART_PROMOPRIJS { get; set; }
    public string? ART_PROMO_DATVAN { get; set; }
    public string? ART_PROMO_DATTOT { get; set; }
    public string? ART_CERTIFICAAT { get; set; }
    public string? ART_PREFAB_ELEMENT { get; set; }
    public string? ART_SERIENUMMER { get; set; }
    public string? ART_MERK_ID { get; set; }
    public string? ART_KEUZELIJST_STOCK_ID { get; set; }
    public string? ART_OPMAAK { get; set; }
    public string? ART_CAR_STOCK { get; set; }
    public string? ART_PRIJSAANVRAAG { get; set; }
    public string? ART_SOM_PRIJSAANVRAAG { get; set; }
    public string? ART_LEVERTERMIJN_DAGEN { get; set; }
    public string? ART_LEVERTERMIJN_MAN_AANGEPAST { get; set; }
    public string? ART_VAN_WORKABOUT { get; set; }
    public string? ART_LAATSTE_VERKOOPDATUM { get; set; }
    public string? ART_SAPA_ID { get; set; }
    public string? ART_MINSTOCK_STUKS { get; set; }
    public string? ART_MAXSTOCK_STUKS { get; set; }
    public string? ART_VELD3 { get; set; }
    public string? ART_WEBSHOP { get; set; }
    public string? ART_REMARK_PLAIN_TEXT { get; set; }

}

public class ArticleWeb
{
    public ArticleWeb() { }

    public ArticleWeb(CebeoItem articleCebeo)
    {
        reflev = articleCebeo.Material?.Reference;
        omschrijving = articleCebeo.Material?.Description;
        merk = articleCebeo.Material?.BrandName;
        aaneh = articleCebeo.UnitOfMeasure;
        minaan = articleCebeo.SalesPackQuantity;
        aankoop = float.Parse(articleCebeo.UnitPrice?.NetPrice ?? "");
        tarief = float.Parse(articleCebeo.UnitPrice?.TarifPrice ?? "");
        link = $"https://www.cebeo.be/catalog/nl-be/products/{merk}-{string.Join('-', Regex.Replace(omschrijving ?? throw new Exception("Article form Cebeo has no description"), "[^0-9A-Za-z _-]", "").Split(' ').Where(x => x != "").Select(x => x.Trim()))}-{articleCebeo.Material?.SupplierItemID ?? throw new Exception("Article form Cebeo has no articleNumber")}".ToLower();
        linkMime = articleCebeo.MimeInfo?.Mime?.Find(m => m.MimeType != null && m.MimeType.Equals("url"))?.MimeSource;
        linkImage = articleCebeo.MimeInfo?.Mime?.Find(m => m.MimeType != null && m.MimeType.Equals("image"))?.MimeSource;
    }

    public string? artikelNr { get; set; }
    public string? reflev { get; set; }
    public string? omschrijving { get; set; }
    public string? familie { get; set; }
    public string? subfamilie { get; set; }
    public float? lengte { get; set; }
    public float? breedte { get; set; }
    public float? hoogte { get; set; }
    public string? omrekfac { get; set; }
    public string? typfac { get; set; }
    public string? merk { get; set; }
    public string? aaneh { get; set; }
    public int? minaan { get; set; }
    public float? aankoop { get; set; }
    public float? verkoop { get; set; }
    public float? tarief { get; set; }
    public float? stdKorting { get; set; }
    public string? hoofdleverancier { get; set; }
    public string? link { get; set; }
    public string? linkMime { get; set; }
    public string? linkImage { get; set; }
}

public class ArticleGroep
{
    public string? ARG_ID { get; set; }
    public string? ARG_OMSCHRIJVING { get; set; }
}