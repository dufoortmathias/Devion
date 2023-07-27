namespace Api.Devion.Helpers.ETS;

public static class ETSCustomerHelper
{
    public static List<CustomersETS> GetCustomers()
    {
        FirebirdClientETS client = new FirebirdClientETS();
        string query = "select K.KL_COD, K.KL_NAM, K.KL_OPV, K.KL_STR, K.KL_PNR, K.KL_WPL, K.KL_LND, K.KL_TEL, K.KL_FAX, K.KL_TEX, K.KL_EMAIL, K.KL_WEBPAGE, K.KL_T, K.KL_BTW, K.KL_TYP, K.KL_BOE, K.KL_VRIJ1, K.KL_MNT, BM.BM_OMS, BV.BVW_CODE, BC.BEL_CODE, R.REK_CODE, KL_SLECHTBET from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID; ";
        string json = client.selectQuery(query);
        List<CustomersETS> customers = JsonTool.ConvertTo<List<CustomersETS>>(json);
        return customers;
    }
}