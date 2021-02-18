using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace KONEPS_KPC_MailBatch
{
    internal class apiRequest
    {
        public void DataCollection(string today)
        {
            selectTechEvalScore(today);
        }
        /// <summary>
        /// Api : 나라장터 입찰공고정보서비스
        /// 인증키 : SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D  (유효기간 24개월)
        /// </summary>
        public void BidPublicInfoService(string today)
        {
            List<List<KONEPSModel>> list = new List<List<KONEPSModel>>();
            List<List<KONEPSModel_Sub>> listSub = new List<List<KONEPSModel_Sub>>();
            List<List<OpengResultListInfoServc>> listOpengResultListInfoServc = new List<List<OpengResultListInfoServc>>();
            List<List<ScsbidListSttusServc>> listScsbidListSttusServc = new List<List<ScsbidListSttusServc>>();

            DateTime T1 = DateTime.Parse(SDate);
            DateTime T2 = DateTime.Parse(EDate);

            TimeSpan TS = T2 - T1;

            for (int i = 0; i < TS.Days; i++)
            {
                string param = "?inqryDiv=1";
                param += "&inqryBgnDt=" + T1.AddDays(i).ToString("yyyyMMdd") + "0000";
                param += "&inqryEndDt=" + T1.AddDays(i).ToString("yyyyMMdd") + "2359";
                param += "&bidNtceNo=" + HttpUtility.UrlEncode("데이터수집", Encoding.UTF8);
                param += "&numOfRows=323"; //max 323
                param += "&ServiceKey=SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D";

                list.Add(getKONEPSList(param, "BidPublicInfoService/getBidPblancListInfoServcPPSSrch", "용역", true)); //용역
                list.Add(getKONEPSList(param, "BidPublicInfoService/getBidPblancListInfoThngPPSSrch", "물품", true)); //물품


                listSub.Add(getKONEPSList_Sub(param, "HrcspSsstndrdInfoService/getPublicPrcureThngInfoServc", "용역", false));  //사전규격

                //개찰결과
                listOpengResultListInfoServc.Add(getOpengResultListInfoServc(param, "ScsbidInfoService/getOpengResultListInfoServc", "용역"));

                //낙찰결과
                listScsbidListSttusServc.Add(getScsbidListSttusServc(param, "ScsbidInfoService/getScsbidListSttusServc", "용역"));
                listScsbidListSttusServc.Add(getScsbidListSttusServc(param, "ScsbidInfoService/getScsbidListSttusThng", "물품"));
                DbConnection dbcon = new DbConnection();
                dbcon.InsertKONEPS(list, today);
                //dbcon.InsertKONEPS_Sub(listSub, today);
                //dbcon.InsertOpengResultListInfoServc(listOpengResultListInfoServc, today);
                //dbcon.InsertScsbidListSttusServc(listScsbidListSttusServc, today);

                //개찰완료 목록
                InsertOpengResultListInfoOpengCompt(today);

                //기술평가결과 (크롤링)
                //selectTechEvalScore(today);
            }

            DbConnection dbcon = new DbConnection();
            DataSet ds = dbcon.getData();

            int cnt = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                try
                {
                    cnt++;
                    if (cnt == 3)
                    {
                        cnt = 0;

                        System.Threading.Thread.Sleep(10000);
                    }

                    string param = "?inqryDiv=4";
                    param += "&bidNtceNo=" + ds.Tables[0].Rows[i]["bidNtceNo"];
                    param += "&ServiceKey=SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D";

                    //개찰결과
                    listOpengResultListInfoServc.Add(getOpengResultListInfoServc(param, "ScsbidInfoService/getOpengResultListInfoServc", "용역"));

                    //낙찰결과
                    listScsbidListSttusServc.Add(getScsbidListSttusServc(param, "ScsbidInfoService/getScsbidListSttusServc", "용역"));
                    listScsbidListSttusServc.Add(getScsbidListSttusServc(param, "ScsbidInfoService/getScsbidListSttusThng", "물품"));

                    dbcon.InsertOpengResultListInfoServc(listOpengResultListInfoServc, today);
                    dbcon.InsertScsbidListSttusServc(listScsbidListSttusServc, today);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            //    //개찰완료 목록
            InsertOpengResultListInfoOpengCompt(today);
            selectTechEvalScore(today);

            #region 주석

            //list.Add(getKONEPSList(param, "BidPublicInfoService/getBidPblancListInfoFrgcpt", "외자", true)); //외자
            //list.Add(getKONEPSList(param, "BidPublicInfoService/getBidPblancListInfoEtcPPSSrch", "기타", true)); //기타
            //list.Add(getKONEPSList(param, "BidPublicInfoService/getBidPblancListInfoThngPPSSrch", "물품", true)); //물품
            //list.Add(getKONEPSList(param, "HrcspSsstndrdInfoService/getPublicPrcureThngInfoThng", "물품", false)); //물품
            //list.Add(getKONEPSList(param, "HrcspSsstndrdInfoService/getPublicPrcureThngInfoFrgcpt", "외자", false)); //외자
            //param += "&" + HttpUtility.UrlEncode("bidNtceNm", Encoding.UTF8) + "=" + HttpUtility.UrlEncode(keywordSplit[i], Encoding.UTF8);
            //외자 조회인데 기타조회와 같은데이터가 조회되므로 다른 Api 사용
            //list.Add(getResults(param, "getBidPblancListInfoFrgcptPPSSrch"));
            //나라장터검색조건에 의한 입찰공고공사조회 : 한국생산성본부와 연관 없는 Api
            //list.Add(getResults(param, "getBidPblancListInfoCnstwkPPSSrch"));

            #endregion 주석
        }

        private void selectTechEvalScore(string date)
        {
            //조회
            DbConnection dbcon = new DbConnection();
            //DataTable dt = dbcon.getTechEvalScore(date);
            DataTable dt = dbcon.getTechEvalScore2(date);

            List<TechEvalScore> list = new List<TechEvalScore>();

            //크롤링
            using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];

                    string sitesource = client.DownloadString(dr["prcbdrBiznoLink"].ToString());

                    if (sitesource.Contains("상호명"))
                    {
                        TechEvalScore model = new TechEvalScore();
                        model.bidNtceNo = dr["bidNtceNo"].ToString();
                        model.bidNtceOrd = dr["bidNtceOrd"].ToString();
                        model.bidClsfcNo = dr["bidClsfcNo"].ToString();
                        model.rbidNo = dr["rbidNo"].ToString();
                        model.prcbdrBizno = dr["prcbdrBizno"].ToString();
                        model.prcbdrBizNm = dr["prcbdrNm"].ToString();


                        TechEvalScore modelSub = getTechEvalScoreModel(sitesource);
                        model.bidPricePoint = modelSub.bidPricePoint;
                        model.tchQvlnPoint = modelSub.tchQvlnPoint;
                        model.gnrlzQvlnPoint = modelSub.gnrlzQvlnPoint;
                        model.openLocation = modelSub.openLocation;
                        model.openDate = modelSub.openDate;

                        list.Add(model);
                    }
                }

                dbcon.setTechEvalScore(list, date);
            }
        }

        private TechEvalScore getTechEvalScoreModel(string sitesource)
        {
            TechEvalScore model = new TechEvalScore();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(sitesource);


            foreach (HtmlNode body in doc.DocumentNode.SelectNodes("//body//div//div//div//tr"))
            {
                List<HtmlNode> list = body.SelectNodes("//td").ToList();

                model.bidPricePoint = list[1] == null ? "" : list[1].InnerText;
                model.tchQvlnPoint = list[2] == null ? "" : list[2].InnerText;
                model.gnrlzQvlnPoint = list[3] == null ? "" : list[3].InnerText;
                if (list.Count > 4)
                {
                    model.openLocation = list[4] == null ? "" : list[4].InnerText.Replace("\r", "").Replace("\n", "").Replace("\t", "");
                    
                }
                if (list.Count > 5)
                {
                    model.openDate = list[5] == null ? "" : list[5].InnerText;
                }

                break;
            }

            return model;
        }

        private void InsertOpengResultListInfoOpengCompt(string date)
        {
            DbConnection dbcon = new DbConnection();
            //DataTable dt = dbcon.getScsbidList(date);
            DataTable dt = dbcon.getScsbidList2(date);
            string url = "ScsbidInfoService/getOpengResultListInfoOpengCompt";
            List<List<OpengResultListInfoOpengCompt>> list = new List<List<OpengResultListInfoOpengCompt>>();

            int cnt = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cnt++;
                if (cnt == 15)
                {
                    cnt = 0;

                    System.Threading.Thread.Sleep(10000);
                }

                string param = "?serviceKey=SfRfqLWT2LlZAqs5Ug3g9ro6HYeA3Xznw8tH%2Bs%2FGzE3exHM46aR%2BFlJgYMcov6dYn3csiT5rG16%2BLVi8IQbYtw%3D%3D";
                param += "&numOfRows=10";
                param += "&pageNo=1";
                param += "&bidNtceNo=" + dt.Rows[i]["bidNtceNo"];
                param += "&bidNtceOrd=" + dt.Rows[i]["bidNtceOrd"];
                param += "&bidClsfcNo=" + dt.Rows[i]["bidClsfcNo"];
                param += "&rbidNo=" + dt.Rows[i]["rbidNo"];

                list.Add(getOpengResultListInfoOpengCompt(param, url));
            }
            dbcon.InsertOpengResultListInfoOpengCompt(list, date);
        }

        private List<OpengResultListInfoOpengCompt> getOpengResultListInfoOpengCompt(string param, string subUrl)
        {
            string url = "http://apis.data.go.kr/1230000/" + subUrl;
            url += param;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                apiXmlToDictionary dic = new apiXmlToDictionary();

                return dic.XmlToDictionary_OpengResultListInfoOpengCompt(results);
            }
        }

        /// <summary>
        /// 나라장터검색조건에 의한 입찰공고 기타조회
        /// 나라장터검색조건에 의한 입찰공고물품조회
        /// 나라장터검색조건에 의한 입찰공고외자조회
        /// 나라장터검색조건에 의한 입찰공고용역조회
        /// 나라장터검색조건에 의한 입찰공고공사조회
        /// </summary>
        private List<KONEPSModel> getResults(string param, string subUrl, string taskName, bool check)
        {
            string url = "http://apis.data.go.kr/1230000/" + subUrl;
            url += param;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                apiXmlToDictionary dic = new apiXmlToDictionary();

                return dic.XmlToDictionaryKONEPS(results, taskName);
            }
        }

        private List<KONEPSModel_Sub> getResults_Sub(string param, string subUrl, string taskName, bool check)
        {
            string url = "http://apis.data.go.kr/1230000/" + subUrl;
            url += param;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                apiXmlToDictionary dic = new apiXmlToDictionary();

                return dic.XmlToDictionary_HrcspSsstndrdInfoService(results, taskName);
            }
        }

        private int getTotalCount(string param, string subUrl)
        {
            string url = "http://apis.data.go.kr/1230000/" + subUrl;
            url += param;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                apiXmlToDictionary dic = new apiXmlToDictionary();

                return dic.XmlToDictionary(results);
            }
        }

        //입찰공고
        private List<KONEPSModel> getKONEPSList(string param, string subUrl, string taskName, bool check)
        {
            List<KONEPSModel> list = new List<KONEPSModel>();

            int count = 0;
            count = getTotalCount(param, subUrl);

            if (count > 323)
            {
                int forCount = (count / 323) + 1;
                for (int i = 0; i < forCount; i++)
                {
                    string pageParam = param + "&pageNo=" + (i + 1);
                    list.AddRange(getResults(pageParam, subUrl, taskName, check));
                }
            }
            else
            {
                list.AddRange(getResults(param, subUrl, taskName, check));
            }

            return list;
        }

        //사전규격
        private List<KONEPSModel_Sub> getKONEPSList_Sub(string param, string subUrl, string taskName, bool check)
        {
            List<KONEPSModel_Sub> list = new List<KONEPSModel_Sub>();

            int count = 0;
            count = getTotalCount(param, subUrl);

            if (count > 323)
            {
                int forCount = (count / 323) + 1;
                for (int i = 0; i < forCount; i++)
                {
                    string pageParam = param + "&pageNo=" + (i + 1);
                    list.AddRange(getResults_Sub(pageParam, subUrl, taskName, check));
                }
            }
            else
            {
                list.AddRange(getResults_Sub(param, subUrl, taskName, check));
            }

            return list;
        }

        //개찰결과
        private List<OpengResultListInfoServc> getOpengResultListInfoServc(string param, string subUrl, string taskName)
        {
            List<OpengResultListInfoServc> list = new List<OpengResultListInfoServc>();

            int count = 0;
            count = getTotalCount(param, subUrl);

            if (count > 323)
            {
                int forCount = (count / 323) + 1;
                for (int i = 0; i < forCount; i++)
                {
                    string pageParam = param + "&pageNo=" + (i + 1);
                    list.AddRange(XmlOpengResultListInfoServc(pageParam, subUrl, taskName));
                }
            }
            else
            {
                list.AddRange(XmlOpengResultListInfoServc(param, subUrl, taskName));
            }

            return list;
        }

        private List<OpengResultListInfoServc> XmlOpengResultListInfoServc(string param, string subUrl, string taskName)
        {
            string url = "http://apis.data.go.kr/1230000/" + subUrl;
            url += param;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                apiXmlToDictionary dic = new apiXmlToDictionary();

                return dic.XmlToDictionary_OpengResultListInfoServc(results, taskName);
            }
        }

        //낙찰결과
        private List<ScsbidListSttusServc> getScsbidListSttusServc(string param, string subUrl, string taskName)
        {
            List<ScsbidListSttusServc> list = new List<ScsbidListSttusServc>();

            int count = 0;
            count = getTotalCount(param, subUrl);

            if (count > 323)
            {
                int forCount = (count / 323) + 1;
                for (int i = 0; i < forCount; i++)
                {
                    string pageParam = param + "&pageNo=" + (i + 1);
                    list.AddRange(XmlScsbidListSttusServc(pageParam, subUrl, taskName));
                }
            }
            else
            {
                list.AddRange(XmlScsbidListSttusServc(param, subUrl, taskName));
            }

            return list;
        }

        private List<ScsbidListSttusServc> XmlScsbidListSttusServc(string param, string subUrl, string taskName)
        {
            string url = "http://apis.data.go.kr/1230000/" + subUrl;
            url += param;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            string results = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                results = reader.ReadToEnd();

                apiXmlToDictionary dic = new apiXmlToDictionary();

                return dic.XmlToDictionary_ScsbidListSttusServc(results, taskName);
            }
        }

        #region 다른 Api 정보

        /// <summary>
        /// Api : 나라장터 수요기관 및 조달업체 정보
        ///         조달업체공급물품정보조회
        ///         조달업체업종정보조회
        ///         조달업체 기본정보
        ///         수요기관정보조회
        /// 인증키 : baFZOEweyT0NIuxSfSFAJTweLh6uEvEcmrbD%2FQ82hNCD0r8ebcokJKf8pOSNfp%2FRsXbKFG8Jncm%2FgL9EXkvHxQ%3D%3D
        /// End Point : http://apis.data.go.kr/1230000/UsrInfoService
        /// </summary>
        private void UsrInfoService()
        {
        }

        /// <summary>
        /// Api : 조달요청정보
        ///         나라장터검색조건에 의한 조달요청 외자조회
        ///         조달요청에 대한 외자세부조회
        ///         조달요청에 대한 외자조회
        ///         나라장터검색조건에 의한 조달요청 기술용역조회
        ///         조달요청에 대한 기술용역조회
        ///         나라장터검색조건에 의한 조달요청 일반용역조회
        ///         조달요청에 대한 일반용역조회
        ///         나라장터검색조건에 의한 조달요청 공사조회
        ///         조달요청에 대한 공사조회
        ///         나라장터검색조건에 의한 조달요청 물품조회
        ///         조달요청에 대한 물품세부조회
        ///         조달요청에 대한 물품조회
        /// 인증키 : baFZOEweyT0NIuxSfSFAJTweLh6uEvEcmrbD%2FQ82hNCD0r8ebcokJKf8pOSNfp%2FRsXbKFG8Jncm%2FgL9EXkvHxQ%3D%3D
        /// End Point : http://apis.data.go.kr/1230000/PrcrmntReqInfoService
        /// </summary>
        private void PrcrmntReqInfoService()
        {
        }

        /// <summary>
        /// Api : 가격정보
        ///         시장시공가격(기계설비) 가격정보
        ///         시장시공가격(건축) 가격정보
        ///         시장시공가격(토목) 가격정보
        ///         시설공통자재(전기, 정보통신) 가격정보
        ///         시설공통자재(기계설비) 가격정보
        ///         시설공통자재(건축) 가격정보
        ///         시설공통자재(토목) 가격정보
        ///         조경수목 가격정보
        /// 인증키 : baFZOEweyT0NIuxSfSFAJTweLh6uEvEcmrbD%2FQ82hNCD0r8ebcokJKf8pOSNfp%2FRsXbKFG8Jncm%2FgL9EXkvHxQ%3D%3D
        /// End Point : http://apis.data.go.kr/1230000/PriceInfoService
        /// </summary>
        private void PriceInfoService()
        {
        }

        #endregion 다른 Api 정보
    }
}