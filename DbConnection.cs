using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace KONEPS_KPC_MailBatch
{
    internal class DbConnection
    {
        private string connectionString = "server = localhost; uid = sa; pwd = 1111; database = PrivateData;";
        //private string connectionString = "server = 10.1.10.16; uid = kpckoneps; pwd = $skfkwkdxj; database = KONEPS_KPC;";
        private string ERP_connectionString = "server = 10.1.10.21; uid = kpckoneps; pwd = $skfkwkdxj; database = KPC;";

        public DataSet ERP_SelectUser()
        {
            string queryString = "select * from kpc_TPEAcKg2bMailKeyWordEnter";
            using (SqlConnection sqlConn = new SqlConnection(ERP_connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter();
                _SqlDataAdapter.SelectCommand = new SqlCommand(queryString, sqlConn);
                _SqlDataAdapter.Fill(ds);

                return ds;
            }
        }

        public void ERP_USER_to_KPC_USER(DataSet ds)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "EXEC ERP_USER_TO_KPC_USER @ID,@EMail,@EMailKeyWord,@EnterDate,@EmpSeq";
                
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = ds.Tables[0].Rows[i];
                    sqlComm.Parameters.AddWithValue("@ID", dr["ID"]);
                    sqlComm.Parameters.AddWithValue("@EMail", dr["EMail"]);
                    sqlComm.Parameters.AddWithValue("@EMailKeyWord", dr["EMailKeyWord"]);
                    sqlComm.Parameters.AddWithValue("@EnterDate", dr["EnterDate"]);
                    sqlComm.Parameters.AddWithValue("@EmpSeq", dr["EmpSeq"]);
                    sqlComm.ExecuteNonQuery();
                    sqlComm.Parameters.Clear();
                }
            }
        }   

        #region 사용자 호출

        /// <summary>
        /// 결과 set [0] : 사용자 email, [1] : 사용자 keyword
        /// </summary>
        /// <returns></returns>
        public List<DbSelectModel> SelectUser()
        {
            string queryString = "select * from KPC_USER where keyword != ''";
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter();
                _SqlDataAdapter.SelectCommand = new SqlCommand(queryString, sqlConn);
                _SqlDataAdapter.Fill(ds);

                List<DbSelectModel> empList = ds.Tables[0].AsEnumerable()
                                .Select(dataRow => new DbSelectModel
                                {
                                    email = dataRow.Field<string>("email"),
                                    keyword = dataRow.Field<string>("keyword")
                                }).ToList();

                return empList;
            }
        }

        #endregion 사용자 호출

        public DataSet getData()
        {
            string queryString = "select distinct * from KONEPS order by bidntceno desc";
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter();
                _SqlDataAdapter.SelectCommand = new SqlCommand(queryString, sqlConn);
                _SqlDataAdapter.Fill(ds);

                return ds;
            }
        }

        #region 호출된 Api 값 DB에 저장

        /// <summary>
        /// 호출된 Api 값 DB에 저장
        /// </summary>
        /// <param name="list"></param>
        /// <param name="createDt"></param>
        public void InsertKONEPS(List<List<KONEPSModel>> list, string createDt)
        {
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    SqlCommand sqlComm = new SqlCommand();
                    sqlComm.Connection = sqlConn;
                    sqlConn.Open();
                    sqlComm.CommandText = "insert into KONEPS (ntceKindNm,bidNtceDtlUrl,cntrctCnclsMthdNm,taskName,bidNtceNm,bidNtceNo,bidNtceOrd,presmptPrce,d2bMngBssamt,bidNtceDt,opengDt,bidBeginDt,bidClseDt,ntceInsttNm,dminsttNm,ntceInsttOfclNm,ntceInsttOfclTelNo,CreateDt,asignBdgtAmt) " +
                        "values (@ntceKindNm,@bidNtceDtlUrl,@cntrctCnclsMthdNm,@taskName,@bidNtceNm,@bidNtceNo,@bidNtceOrd,@presmptPrce,@d2bMngBssamt,@bidNtceDt,@opengDt,@bidBeginDt,@bidClseDt,@ntceInsttNm,@dminsttNm,@ntceInsttOfclNm,@ntceInsttOfclTelNo,@CreateDt,@asignBdgtAmt)";

                    for (int i = 0; i < list.Count; i++)
                    {
                        for (int j = 0; j < list[i].Count; j++)
                        {
                            KONEPSModel model = list[i][j];
                            sqlComm.Parameters.AddWithValue("@ntceKindNm", model.ntceKindNm == null ? "" : model.ntceKindNm);
                            sqlComm.Parameters.AddWithValue("@bidNtceDtlUrl", model.bidNtceDtlUrl == null ? "" : model.bidNtceDtlUrl);
                            sqlComm.Parameters.AddWithValue("@cntrctCnclsMthdNm", model.cntrctCnclsMthdNm == null ? "" : model.cntrctCnclsMthdNm);
                            sqlComm.Parameters.AddWithValue("@taskName", model.taskName == null ? "" : model.taskName);
                            sqlComm.Parameters.AddWithValue("@bidNtceNm", model.bidNtceNm == null ? "" : model.bidNtceNm);
                            sqlComm.Parameters.AddWithValue("@bidNtceNo", model.bidNtceNo == null ? "" : model.bidNtceNo);
                            sqlComm.Parameters.AddWithValue("@bidNtceOrd", model.bidNtceOrd == null ? "" : model.bidNtceOrd);
                            sqlComm.Parameters.AddWithValue("@presmptPrce", model.presmptPrce == null ? "" : model.presmptPrce);
                            sqlComm.Parameters.AddWithValue("@d2bMngBssamt", model.d2bMngBssamt == null ? "" : model.d2bMngBssamt);
                            sqlComm.Parameters.AddWithValue("@bidNtceDt", model.bidNtceDt == null ? "" : model.bidNtceDt);
                            sqlComm.Parameters.AddWithValue("@opengDt", model.opengDt == null ? "" : model.opengDt);
                            sqlComm.Parameters.AddWithValue("@bidBeginDt", model.bidBeginDt == null ? "" : model.bidBeginDt);
                            sqlComm.Parameters.AddWithValue("@bidClseDt", model.bidClseDt == null ? "" : model.bidClseDt);
                            sqlComm.Parameters.AddWithValue("@ntceInsttNm", model.ntceInsttNm == null ? "" : model.ntceInsttNm);
                            sqlComm.Parameters.AddWithValue("@dminsttNm", model.dminsttNm == null ? "" : model.dminsttNm);
                            sqlComm.Parameters.AddWithValue("@ntceInsttOfclNm", model.ntceInsttOfclNm == null ? "" : model.ntceInsttOfclNm);
                            sqlComm.Parameters.AddWithValue("@ntceInsttOfclTelNo", model.ntceInsttOfclTelNo == null ? "" : model.ntceInsttOfclTelNo);
                            sqlComm.Parameters.AddWithValue("@CreateDt", createDt);
                            sqlComm.Parameters.AddWithValue("@asignBdgtAmt", model.asignBdgtAmt == null ? "" : model.asignBdgtAmt);
                            sqlComm.ExecuteNonQuery();
                            sqlComm.Parameters.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void InsertKONEPS_Sub(List<List<KONEPSModel_Sub>> list, string createDt)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "insert into KONEPS_Sub (prdctClsfcNoNm,bfSpecRgstNo,asignBdgtAmt,rcptDt,opninRgstClseDt,orderInsttNm,rlDminsttNm,ofclNm,ofclTelNo,taskName,CreateDt,bidNtceDtlUrl) " +
                    "values (@prdctClsfcNoNm,@bfSpecRgstNo,@asignBdgtAmt,@rcptDt,@opninRgstClseDt,@orderInsttNm,@rlDminsttNm,@ofclNm,@ofclTelNo,@taskName,@CreateDt,@bidNtceDtlUrl)";

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        KONEPSModel_Sub model = list[i][j];
                        sqlComm.Parameters.AddWithValue("@prdctClsfcNoNm", model.prdctClsfcNoNm == null ? "" : model.prdctClsfcNoNm);
                        sqlComm.Parameters.AddWithValue("@bfSpecRgstNo", model.bfSpecRgstNo == null ? "" : model.bfSpecRgstNo);
                        sqlComm.Parameters.AddWithValue("@asignBdgtAmt", model.asignBdgtAmt == null ? "" : model.asignBdgtAmt);
                        sqlComm.Parameters.AddWithValue("@rcptDt", model.rcptDt == null ? "" : model.rcptDt);
                        sqlComm.Parameters.AddWithValue("@opninRgstClseDt", model.opninRgstClseDt == null ? "" : model.opninRgstClseDt);
                        sqlComm.Parameters.AddWithValue("@orderInsttNm", model.orderInsttNm == null ? "" : model.orderInsttNm);
                        sqlComm.Parameters.AddWithValue("@rlDminsttNm", model.rlDminsttNm == null ? "" : model.rlDminsttNm);
                        sqlComm.Parameters.AddWithValue("@ofclNm", model.ofclNm == null ? "" : model.ofclNm);
                        sqlComm.Parameters.AddWithValue("@ofclTelNo", model.ofclTelNo == null ? "" : model.ofclTelNo);
                        sqlComm.Parameters.AddWithValue("@taskName", model.taskName == null ? "" : model.taskName);
                        sqlComm.Parameters.AddWithValue("@CreateDt", createDt);
                        sqlComm.Parameters.AddWithValue("@bidNtceDtlUrl", model.bidNtceDtlUrl == null ? "" : model.bidNtceDtlUrl); 
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                }
            }
        }

        public void InsertOpengResultListInfoServc(List<List<OpengResultListInfoServc>> list, string createDt)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "insert into OpengResultListInfoServc (taskName,bidNtceNo,bidNtceOrd,bidClsfcNo,rbidNo,bidNtceNm,dminsttNm,opengDt,prtcptCnum,opengCorpInfo,enterName,bddprDlamt,bddprRT,progrsDivCdNm,CreateDt) " +
                    "values (@taskName,@bidNtceNo,@bidNtceOrd,@bidClsfcNo,@rbidNo,@bidNtceNm,@dminsttNm,@opengDt,@prtcptCnum,@opengCorpInfo,@enterName,@bddprDlamt,@bddprRT,@progrsDivCdNm,@CreateDt)";

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        OpengResultListInfoServc model = list[i][j];
                        sqlComm.Parameters.AddWithValue("@bidNtceNo", model.bidNtceNo == null ? "" : model.bidNtceNo);
                        sqlComm.Parameters.AddWithValue("@bidNtceOrd", model.bidNtceOrd == null ? "" : model.bidNtceOrd);
                        sqlComm.Parameters.AddWithValue("@bidClsfcNo", model.bidClsfcNo == null ? "" : model.bidClsfcNo);
                        sqlComm.Parameters.AddWithValue("@rbidNo", model.rbidNo == null ? "" : model.rbidNo);
                        sqlComm.Parameters.AddWithValue("@bidNtceNm", model.bidNtceNm == null ? "" : model.bidNtceNm);
                        sqlComm.Parameters.AddWithValue("@dminsttNm", model.dminsttNm == null ? "" : model.dminsttNm);
                        sqlComm.Parameters.AddWithValue("@opengDt", model.opengDt == null ? "" : model.opengDt);
                        sqlComm.Parameters.AddWithValue("@prtcptCnum", model.prtcptCnum == null ? "" : model.prtcptCnum);
                        sqlComm.Parameters.AddWithValue("@opengCorpInfo", model.opengCorpInfo == null ? "" : model.opengCorpInfo);
                        sqlComm.Parameters.AddWithValue("@enterName", model.enterName == null ? "" : model.enterName);
                        sqlComm.Parameters.AddWithValue("@bddprDlamt", model.bddprDlamt == null ? "" : model.bddprDlamt);
                        sqlComm.Parameters.AddWithValue("@bddprRT", model.bddprRT == null ? "" : model.bddprRT);
                        sqlComm.Parameters.AddWithValue("@progrsDivCdNm", model.progrsDivCdNm == null ? "" : model.progrsDivCdNm);
                        sqlComm.Parameters.AddWithValue("@taskName", model.taskName == null ? "" : model.taskName);
                        sqlComm.Parameters.AddWithValue("@CreateDt", createDt);
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                }
            }
        }

        public void InsertScsbidListSttusServc(List<List<ScsbidListSttusServc>> list, string createDt)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "insert into ScsbidListSttusServc (taskName,bidNtceNo,bidNtceOrd,bidClsfcNo,rbidNo,bidNtceNm,dminsttNm,rlOpengDt,prtcptCnum,bidwinnrNm,sucsfbidAmt,sucsfbidRate,CreateDt) " +
                    "values (@taskName,@bidNtceNo,@bidNtceOrd,@bidClsfcNo,@rbidNo,@bidNtceNm,@dminsttNm,@rlOpengDt,@prtcptCnum,@bidwinnrNm,@sucsfbidAmt,@sucsfbidRate,@CreateDt)";

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        ScsbidListSttusServc model = list[i][j];
                        sqlComm.Parameters.AddWithValue("@bidNtceNo", model.bidNtceNo == null ? "" : model.bidNtceNo);
                        sqlComm.Parameters.AddWithValue("@bidNtceOrd", model.bidNtceOrd == null ? "" : model.bidNtceOrd);
                        sqlComm.Parameters.AddWithValue("@bidClsfcNo", model.bidClsfcNo == null ? "" : model.bidClsfcNo);
                        sqlComm.Parameters.AddWithValue("@rbidNo", model.rbidNo == null ? "" : model.rbidNo);
                        sqlComm.Parameters.AddWithValue("@bidNtceNm", model.bidNtceNm == null ? "" : model.bidNtceNm);
                        sqlComm.Parameters.AddWithValue("@dminsttNm", model.dminsttNm == null ? "" : model.dminsttNm);
                        sqlComm.Parameters.AddWithValue("@rlOpengDt", model.rlOpengDt == null ? "" : model.rlOpengDt);
                        sqlComm.Parameters.AddWithValue("@prtcptCnum", model.prtcptCnum == null ? "" : model.prtcptCnum);
                        sqlComm.Parameters.AddWithValue("@bidwinnrNm", model.bidwinnrNm == null ? "" : model.bidwinnrNm);
                        sqlComm.Parameters.AddWithValue("@sucsfbidAmt", model.sucsfbidAmt == null ? "" : model.sucsfbidAmt);
                        sqlComm.Parameters.AddWithValue("@sucsfbidRate", model.sucsfbidRate == null ? "" : model.sucsfbidRate);
                        sqlComm.Parameters.AddWithValue("@taskName", model.taskName == null ? "" : model.taskName);
                        sqlComm.Parameters.AddWithValue("@CreateDt", createDt);
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                }
            }
        }

        #endregion 호출된 Api 값 DB에 저장

        #region 사용자 키워드, 날짜 저장된 DB에서 조회

        public DataTable getKONEPS_KeywordList(string date, string keyword)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("EXEC KONEP_KEYWORD_LIST '" + keyword + "','" + date + "'", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }

        public DataTable getKONEPS_SUB_KeywordList(string date, string keyword)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("EXEC KONEP_SUB_KEYWORD_LIST '" + keyword + "','" + date + "'", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }
        #endregion 사용자 키워드, 날짜 저장된 DB에서 조회

        public void setDaliyMailCheck(string mail, string check, string today)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "insert into DALIY_MAIL_CHECK (Email, MAIL_CHECK, CreateDt) " +
                    "values (@Email, @MAIL_CHECK, @CreateDt)";

                sqlComm.Parameters.AddWithValue("@Email", mail);
                sqlComm.Parameters.AddWithValue("@MAIL_CHECK", check);
                sqlComm.Parameters.AddWithValue("@CreateDt", today);

                sqlComm.ExecuteNonQuery();
            }
        }

        public DataTable getScsbidList(string date)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("EXEC SCSBID_LIST '" + date + "'", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }

        public DataTable getScsbidList2(string date)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("select distinct bidNtceNo, bidNtceOrd, bidClsfcNo, rbidNo from ScsbidListSttusServc", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }

        public void InsertOpengResultListInfoOpengCompt(List<List<OpengResultListInfoOpengCompt>> list, string createDt)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "insert into OpengResultListInfoOpengCompt (bidNtceNo,bidNtceOrd,bidClsfcNo,rbidNo,opengRank,prcbdrBizno,prcbdrNm,prcbdrCeoNm,bidprcAmt,bidprcrt,drwtNo1,drwtNo2,bidprcDt,rmrk,prcbdrBiznoLink,CreateDt) " +
                    "values (@bidNtceNo,@bidNtceOrd,@bidClsfcNo,@rbidNo,@opengRank,@prcbdrBizno,@prcbdrNm,@prcbdrCeoNm,@bidprcAmt,@bidprcrt,@drwtNo1,@drwtNo2,@bidprcDt,@rmrk,@prcbdrBiznoLink,@CreateDt)";

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].Count; j++)
                    {
                        OpengResultListInfoOpengCompt model = list[i][j];
                        sqlComm.Parameters.AddWithValue("@bidNtceNo", model.bidNtceNo == null ? "" : model.bidNtceNo);
                        sqlComm.Parameters.AddWithValue("@bidNtceOrd", model.bidNtceOrd == null ? "" : model.bidNtceOrd);
                        sqlComm.Parameters.AddWithValue("@bidClsfcNo", model.bidClsfcNo == null ? "" : model.bidClsfcNo);
                        sqlComm.Parameters.AddWithValue("@rbidNo", model.rbidNo == null ? "" : model.rbidNo);
                        sqlComm.Parameters.AddWithValue("@opengRank", model.opengRank == null ? "" : model.opengRank);
                        sqlComm.Parameters.AddWithValue("@prcbdrBizno", model.prcbdrBizno == null ? "" : model.prcbdrBizno);
                        sqlComm.Parameters.AddWithValue("@prcbdrNm", model.prcbdrNm == null ? "" : model.prcbdrNm);
                        sqlComm.Parameters.AddWithValue("@prcbdrCeoNm", model.prcbdrCeoNm == null ? "" : model.prcbdrCeoNm);
                        sqlComm.Parameters.AddWithValue("@bidprcAmt", model.bidprcAmt == null ? "" : model.bidprcAmt);
                        sqlComm.Parameters.AddWithValue("@bidprcrt", model.bidprcrt == null ? "" : model.bidprcrt);
                        sqlComm.Parameters.AddWithValue("@drwtNo1", model.drwtNo1 == null ? "" : model.drwtNo1);
                        sqlComm.Parameters.AddWithValue("@drwtNo2", model.drwtNo2 == null ? "" : model.drwtNo2);
                        sqlComm.Parameters.AddWithValue("@bidprcDt", model.bidprcDt == null ? "" : model.bidprcDt);
                        sqlComm.Parameters.AddWithValue("@rmrk", model.rmrk == null ? "" : model.rmrk);
                        sqlComm.Parameters.AddWithValue("@prcbdrBiznoLink", model.prcbdrBiznoLink == null ? "" : model.prcbdrBiznoLink);
                        sqlComm.Parameters.AddWithValue("@CreateDt", createDt);
                        sqlComm.ExecuteNonQuery();
                        sqlComm.Parameters.Clear();
                    }
                }
            }
        }

        public DataTable getTechEvalScore(string date)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("SELECT * FROM OpengResultListInfoOpengCompt where CreateDt = '"+ date + "'", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }

        public DataTable getTechEvalScore2(string date)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("SELECT * FROM OpengResultListInfoOpengCompt", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }

        public void setTechEvalScore(List<TechEvalScore> list, string createDt)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlCommand sqlComm = new SqlCommand();
                sqlComm.Connection = sqlConn;
                sqlConn.Open();
                sqlComm.CommandText = "insert into TechEvalScore (bidNtceNo,bidNtceOrd,bidClsfcNo,rbidNo,prcbdrBizno,prcbdrBizNm,bidPricePoint,tchQvlnPoint,gnrlzQvlnPoint,openLocation,openDate,CreateDt) " +
                    "values (@bidNtceNo,@bidNtceOrd,@bidClsfcNo,@rbidNo,@prcbdrBizno,@prcbdrBizNm,@bidPricePoint,@tchQvlnPoint,@gnrlzQvlnPoint,@openLocation,@openDate,@CreateDt)";

                for (int i = 0; i < list.Count; i++)
                {
                    TechEvalScore model = list[i];

                    sqlComm.Parameters.AddWithValue("@bidNtceNo", model.bidNtceNo == null ? "" : model.bidNtceNo);
                    sqlComm.Parameters.AddWithValue("@bidNtceOrd", model.bidNtceOrd == null ? "" : model.bidNtceOrd);
                    sqlComm.Parameters.AddWithValue("@bidClsfcNo", model.bidClsfcNo == null ? "" : model.bidClsfcNo);
                    sqlComm.Parameters.AddWithValue("@rbidNo", model.rbidNo == null ? "" : model.rbidNo);
                    sqlComm.Parameters.AddWithValue("@prcbdrBizno", model.prcbdrBizno == null ? "" : model.prcbdrBizno);
                    sqlComm.Parameters.AddWithValue("@prcbdrBizNm", model.prcbdrBizNm == null ? "" : model.prcbdrBizNm);
                    sqlComm.Parameters.AddWithValue("@bidPricePoint", model.bidPricePoint == null ? "" : model.bidPricePoint);
                    sqlComm.Parameters.AddWithValue("@tchQvlnPoint", model.tchQvlnPoint == null ? "" : model.tchQvlnPoint);
                    sqlComm.Parameters.AddWithValue("@gnrlzQvlnPoint", model.gnrlzQvlnPoint == null ? "" : model.gnrlzQvlnPoint);
                    sqlComm.Parameters.AddWithValue("@openLocation", model.openLocation == null ? "" : model.openLocation);
                    sqlComm.Parameters.AddWithValue("@openDate", model.openDate == null ? "" : model.openDate);
                    sqlComm.Parameters.AddWithValue("@CreateDt", createDt);

                    sqlComm.ExecuteNonQuery();
                    sqlComm.Parameters.Clear();
                }
            }
        }

        public DataTable getDataCollection(string gubun)
        {
            using (SqlConnection sqlConn = new SqlConnection(connectionString))
            {
                SqlDataAdapter _SqlDataAdapter = new SqlDataAdapter("SELECT bidNtceNo FROM KONEPS WHERE taskName = '" + gubun + "'", sqlConn);

                DataTable dt = new DataTable();
                _SqlDataAdapter.Fill(dt);

                return dt;
            }
        }
    }
}