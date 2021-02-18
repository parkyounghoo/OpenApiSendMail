using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KONEPS_KPC_MailBatch
{
    internal class apiXmlToDictionary
    {
        /// <summary>
        /// 기본적인 XmlParse
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public int XmlToDictionary(string results)
        {
            int totalCount = 0;
            XDocument doc = XDocument.Parse(results);
            Dictionary<string, string> dataDictionary = new Dictionary<string, string>();

            foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
            {
                int keyInt = 0;
                string keyName = element.Name.LocalName;

                if (keyName == "totalCount")
                {
                    totalCount = int.Parse(element.Value);

                    break;
                }
            }

            return totalCount;
        }

        /// <summary>
        /// 나라장터 입찰공고용 XmlParse
        /// </summary>
        /// <param name="results"></param>
        /// <returns></returns>
        public List<KONEPSModel> XmlToDictionaryKONEPS(string results, string taskName)
        {
            XDocument doc = XDocument.Parse(results);

            var itemList = from r in doc.Descendants("item")
                           select new KONEPSModel
                           {
                               ntceKindNm = r.Element("ntceKindNm").Value,
                               bidNtceDtlUrl = r.Element("bidNtceDtlUrl").Value,
                               ntceInsttNm = r.Element("ntceInsttNm").Value,
                               dminsttNm = r.Element("dminsttNm").Value,
                               cntrctCnclsMthdNm = r.Element("cntrctCnclsMthdNm").Value,
                               taskName = taskName,
                               bidNtceNm = r.Element("bidNtceNm").Value,
                               bidNtceNo = r.Element("bidNtceNo").Value,
                               bidNtceOrd = r.Element("bidNtceOrd").Value,
                               presmptPrce = r.Element("presmptPrce").Value,
                               d2bMngBssamt = r.Element("d2bMngBssamt").Value,
                               bidNtceDt = r.Element("bidNtceDt").Value,
                               opengDt = r.Element("opengDt").Value,
                               bidBeginDt = r.Element("bidBeginDt").Value,
                               bidClseDt = r.Element("bidClseDt").Value,
                               ntceInsttOfclNm = r.Element("ntceInsttOfclNm").Value,
                               ntceInsttOfclTelNo = r.Element("ntceInsttOfclTelNo").Value,
                               asignBdgtAmt = r.Element("asignBdgtAmt").Value,
                           };

            return itemList.ToList();
        }

        public List<KONEPSModel_Sub> XmlToDictionary_HrcspSsstndrdInfoService(string results, string taskName)
        {
            XDocument doc = XDocument.Parse(results);

            var itemList = from r in doc.Descendants("item")
                           select new KONEPSModel_Sub
                           {
                               prdctClsfcNoNm = r.Element("prdctClsfcNoNm").Value,
                               bfSpecRgstNo = r.Element("bfSpecRgstNo").Value,
                               asignBdgtAmt = r.Element("asignBdgtAmt").Value,
                               rcptDt = r.Element("rcptDt").Value,
                               opninRgstClseDt = r.Element("opninRgstClseDt").Value,
                               orderInsttNm = r.Element("orderInsttNm").Value,
                               rlDminsttNm = r.Element("rlDminsttNm").Value,
                               ofclNm = r.Element("ofclNm").Value,
                               ofclTelNo = r.Element("ofclTelNo").Value,
                               bidNtceDtlUrl = "https://www.g2b.go.kr:8143/ep/preparation/prestd/preStdDtl.do?preStdRegNo=" + r.Element("bfSpecRgstNo").Value,
                               taskName = taskName,
                           };

            return itemList.ToList();
        }

        public List<OpengResultListInfoServc> XmlToDictionary_OpengResultListInfoServc(string results, string taskName)
        {
            XDocument doc = XDocument.Parse(results);
            var itemList = from r in doc.Descendants("item")
                           select new OpengResultListInfoServc
                           {
                               bidNtceNo = r.Element("bidNtceNo").Value,
                               bidNtceOrd = r.Element("bidNtceOrd").Value,
                               bidClsfcNo = r.Element("bidClsfcNo").Value,
                               rbidNo = r.Element("rbidNo").Value,
                               bidNtceNm = r.Element("bidNtceNm").Value,
                               dminsttNm = r.Element("dminsttNm").Value,
                               opengDt = r.Element("opengDt").Value,
                               prtcptCnum = r.Element("prtcptCnum").Value,
                               opengCorpInfo = r.Element("opengCorpInfo").Value,
                               enterName = CorpInfoSplit(r.Element("opengCorpInfo").Value, 0),
                               bddprDlamt = CorpInfoSplit(r.Element("opengCorpInfo").Value, 3),
                               bddprRT = CorpInfoSplit(r.Element("opengCorpInfo").Value, 4),
                               progrsDivCdNm = r.Element("progrsDivCdNm").Value,
                               taskName = taskName,
                           };

            return itemList.ToList();
        }

        private string CorpInfoSplit(string opengCorpInfo, int number)
        {
            //업체명 : 0
            //사업자번호 : 1
            //대표자명 : 2
            //투찰금액 : 3
            //투찰율 : 4
           
            char val = '^';
            if (opengCorpInfo == null)
            {
                return "";
            }
            else
            {
                string[] dataSplit = opengCorpInfo.Split(val);

                if (dataSplit.Length > number)
                {
                    return dataSplit[number] == null ? "" : dataSplit[number];
                }
                else
                {
                    return "";
                }
            }
        }

        public List<ScsbidListSttusServc> XmlToDictionary_ScsbidListSttusServc(string results, string taskName)
        {
            XDocument doc = XDocument.Parse(results);

            var itemList = from r in doc.Descendants("item")
                           select new ScsbidListSttusServc
                           {
                               bidNtceNo = r.Element("bidNtceNo").Value,
                               bidNtceOrd = r.Element("bidNtceOrd").Value,
                               bidClsfcNo = r.Element("bidClsfcNo").Value,
                               rbidNo = r.Element("rbidNo").Value,
                               bidNtceNm = r.Element("bidNtceNm").Value,
                               dminsttNm = r.Element("dminsttNm").Value,
                               rlOpengDt = r.Element("rlOpengDt").Value,
                               prtcptCnum = r.Element("prtcptCnum").Value,
                               bidwinnrNm = r.Element("bidwinnrNm").Value,
                               sucsfbidAmt = r.Element("sucsfbidAmt").Value,
                               sucsfbidRate = r.Element("sucsfbidRate").Value,
                               taskName = taskName,
                           };

            return itemList.ToList();
        }

        public List<OpengResultListInfoOpengCompt> XmlToDictionary_OpengResultListInfoOpengCompt(string results)
        {
            XDocument doc = XDocument.Parse(results);

            var itemList = from r in doc.Descendants("item")
                           select new OpengResultListInfoOpengCompt
                           {
                               bidNtceNo = r.Element("bidNtceNo").Value,
                               bidNtceOrd = r.Element("bidNtceOrd").Value,
                               bidClsfcNo = r.Element("bidClsfcNo").Value,
                               rbidNo = r.Element("rbidNo").Value,
                               opengRank = r.Element("opengRank").Value,
                               prcbdrBizno = r.Element("prcbdrBizno").Value,
                               prcbdrNm = r.Element("prcbdrNm").Value,
                               prcbdrCeoNm = r.Element("prcbdrCeoNm").Value,
                               bidprcAmt = r.Element("bidprcAmt").Value,
                               bidprcrt = r.Element("bidprcrt").Value,
                               drwtNo1 = r.Element("drwtNo1").Value,
                               drwtNo2 = r.Element("drwtNo2").Value,
                               bidprcDt = r.Element("bidprcDt").Value,
                               rmrk = r.Element("rmrk").Value,
                               prcbdrBiznoLink = " http://www.g2b.go.kr:8101/ep/result/selectTechEvalScore.do?bidno="+ r.Element("bidNtceNo").Value + "&bidseq=" + r.Element("bidNtceOrd").Value + "" +
                               "&bidcate=" + r.Element("bidClsfcNo").Value + "&bizRegNo=" + r.Element("prcbdrBizno").Value,
                           };

            return itemList.ToList();
        }
    }
}