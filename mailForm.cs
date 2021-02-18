using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace test
{
    internal class mailForm
    {
        /// <summary>
        /// 검색일자 :
        /// 검색어 :
        /// 공고번호-차수 bidNtceNo - bidNtceOrd
        /// 분류 - ntceKindNm
        /// 공고명 - bidNtceNm : link (bidNtceDtlUrl) : keyword 빨간색표시
        /// 공고기관 - ntceInsttNm
        /// 수요기관 - dminsttNm
        /// 계약방법 - cntrctCnclsMthdNm
        /// 입찰개시일시(입찰마감일시) bidBeginDt (bidClseDt)
        /// </summary>
        /// <param name="mailList">api</param>
        /// <returns></returns>
        public string BidPublicInfoServiceMailForm(DataTable dt, DataTable dtSub, string keyword, string date)
        {
            StringBuilder mailForm = new StringBuilder();

            #region MailForm
            mailForm.Append("<span style='font-weight: bold;'>[ 사전규격 - " + dtSub.Rows.Count + "건 ]</span>");
            mailForm.Append("<table style='margin-bottom:40px;border:1px solid rgb(230, 230, 230); border-image:none; width:100%; text-align:center; color:rgb(88, 88, 88); font-size:11.5px; margin-top:5px; border-collapse:collapse; table-layout:fixed; -ms-overflow-y:auto; max-height:200px;'>");
            mailForm.Append("<tbody>");
            mailForm.Append("<tr style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid;background-color:cadetblue;font-size:12.5px;color:white'>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:40px; height:50px; font-weight:bold; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>번호</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:250px; height:50px; font-weight:bold; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>품목명/공고명<br>사전규격등록번호</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:100px; height:50px; font-weight:bold; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>배정예산</br>금액(원)</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:125px; height:50px; font-weight:bold; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>접수일시<br>의견등록마감일시</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:135px; height:50px; font-weight:bold; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>발주기관<br>실수요기관</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:132px; height:50px; font-weight:bold; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>담당자<br>담당자연락처</td>");
            mailForm.Append("</tr>");
            for (int i = 0; i < dtSub.Rows.Count; i++)
            {
                DataRow dr = dtSub.Rows[i];
                mailForm.Append("<tr style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid;'>");
                mailForm.Append("<td rowspan='2' style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:40px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>" + dr["SEQ_NO"] + "</td>");

                mailForm.Append("<td rowspan='2' style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:250px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'><a href=" + dr["bidNtceDtlUrl"] + " target='_blank'>");
                string mailFormComplete = dr["prdctClsfcNoNm"] + "</br> " + dr["bfSpecRgstNo"];
                string[] keywordSplit = keyword.Split('/');
                for (int z = 0; z < keywordSplit.Count(); z++)
                {
                    if (keywordSplit[z] != "")
                    {
                        if (mailFormComplete.Contains(keywordSplit[z]))
                        {
                            mailFormComplete = mailFormComplete.Replace(keywordSplit[z], "<label style='color:black; background-color:lightblue;font-color'>" + keywordSplit[z] + "</label>");
                        }
                        else
                        {
                            mailFormComplete = mailFormComplete.Replace(keywordSplit[z].ToLower(), "<label style='color:black; background-color:lightblue;font-color'>" + keywordSplit[z].ToLower() + "</label>");
                        }
                    } 
                }
                mailForm.Append(mailFormComplete);
                mailForm.Append("</a></td>");
                //mailForm.Append("<td style='width: 40px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>" + dr["prdctClsfcNoNm"] + "</br> " + dr["bfSpecRgstNo"] + "</td>");
                mailForm.Append("<td rowspan='2' style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:100px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;'>" + getIntCheck(dr["asignBdgtAmt"].ToString()) + "</td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:125px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;font-size:10px'>" + dr["rcptDt"]  + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:135px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;font-size:11px'>" + dr["orderInsttNm"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:132px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;font-size:11px'>" + dr["ofclNm"] + " </td>");
                mailForm.Append("</tr>");
                mailForm.Append("<tr style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid;'>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:135px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;font-size:10px'>" + dr["opninRgstClseDt"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:135px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;font-size:11px'>"+ dr["rlDminsttNm"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width:135px; height:50px; border-right-color:rgb(230, 230, 230); border-right-width:1px; border-right-style:solid;font-size:11px'>" +dr["ofclTelNo"] + " </td>");
                mailForm.Append("</tr>");
            }
            mailForm.Append("</tbody>");
            mailForm.Append("</table>");
            mailForm.Append("</div>");
            mailForm.Append("<div style='margin: 20px auto 0px; width: 800px;'>");
            mailForm.Append("<span style='font-weight: bold;'>[ 입찰정보 - " + dt.Rows.Count + "건 ]</span>");
            mailForm.Append("<table style='border: 1px solid rgb(230, 230, 230); border-image: none; width: 100%; text-align: center; color: rgb(88, 88, 88); font-size: 11.5px; margin-top: 5px; border-collapse: collapse; table-layout: fixed; -ms-overflow-y: auto; max-height: 200px;'>");
            mailForm.Append("<tbody>");
            mailForm.Append("<tr style='border-bottom-color: rgb(230, 230, 230); border-bottom-width: 1px; border-bottom-style: solid;background-color:cadetblue;font-size:12.5px;color:white'>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 40px; height: 50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>번호</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 250px; height:50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>공고명<br>입찰공고번호-입찰공고차수</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 95px; height:50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>배정예산</br>금액(원)</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 100px;height:50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>공고일시<br>개찰일시</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 100px;height:50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>개시일시<br>마감일시</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 97px; height:50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>발주기관<br>수요기관</td>");
            mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 97px; height:50px; font-weight: bold; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>담당자<br>담당자연락처</td>");
            mailForm.Append("</tr>");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                mailForm.Append("<tr style='border-bottom-color: rgb(230, 230, 230); border-bottom-width: 1px; border-bottom-style: solid;';>");
                mailForm.Append("<td rowspan='2' style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 40px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;';>" + dr["SEQ_NO"] + "</td>");

                mailForm.Append("<td rowspan='2' style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 250px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;';><a href=" + dr["bidNtceDtlUrl"] + " target='_blank'>");
                string mailFormComplete = dr["bidNtceNm"] + "</br> " + dr["bidNtceNo"] + "-" + dr["bidNtceOrd"];
                string[] keywordSplit = keyword.Split('/');
                for (int z = 0; z < keywordSplit.Count(); z++)
                {
                    if (keywordSplit[z] != "")
                    {
                        if (mailFormComplete.Contains(keywordSplit[z]))
                        {
                            mailFormComplete = mailFormComplete.Replace(keywordSplit[z], "<label style='color:black; background-color:lightblue;font-color'>" + keywordSplit[z] + "</label>");
                        }
                        else
                        {
                            mailFormComplete = mailFormComplete.Replace(keywordSplit[z].ToLower(), "<label style='color:black; background-color:lightblue;font-color'>" + keywordSplit[z].ToLower() + "</label>");
                        }
                    }
                }
                mailForm.Append(mailFormComplete);
                mailForm.Append("</a></td>");
                //mailForm.Append("<td style='width: 40px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;';>" + dr["bidNtceNm"] + "</br> " + dr["bidNtceNo"] + "-"+ dr["bidNtceOrd"] + " </td>");
                mailForm.Append("<td rowspan='2' style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 95px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;'>" + getIntCheck(dr["asignBdgtAmt"].ToString()) + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 100px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:10px'>" + dr["bidNtceDt"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 100px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:10px'>" + dr["bidBeginDt"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 97px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:11px'>" + dr["ntceInsttNm"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 97px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:11px'>" + dr["ntceInsttOfclNm"] + " </td>");
                mailForm.Append("</tr>");
                mailForm.Append("<tr style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid;'>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 95px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:10px'>" + dr["opengDt"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 95px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:10px'>" + dr["bidClseDt"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 95px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:11px'>" + dr["dminsttNm"] + " </td>");
                mailForm.Append("<td style='border-bottom-color:rgb(230, 230, 230); border-bottom-width:1px; border-bottom-style:solid; width: 95px; height: 50px; border-right-color: rgb(230, 230, 230); border-right-width: 1px; border-right-style: solid;font-size:11px'>" + dr["ntceInsttOfclTelNo"] + " </td>");
                mailForm.Append("</tr>");
            }
            mailForm.Append("</tbody>");
            mailForm.Append("</table>");
            mailForm.Append("</div>");
            mailForm.Append("<br />");
            mailForm.Append("<font style='font-size:12px; font-family:'Malgun Gothic''>감사합니다.</font>");

            #endregion MailForm

            return mailForm.ToString();
        }

        /// <summary>
        /// 메일 발송
        /// </summary>
        /// <param name="mailForm"></param>
        /// <param name="mailTo"></param>
        public void SendMail(string mailForm, string mailTo, string today)
        {
            if (mailForm.Length < 9000000 )
            {
                MailMessage msg = new MailMessage("test", mailTo);
                msg.Subject = "나라장터 입찰정보(사전규격/입찰)";
                msg.IsBodyHtml = true;
                msg.Body = mailForm;

                using (SmtpClient client = new SmtpClient("124.111.37.73", 25))
                {
                    //client.EnableSsl = true;
                    //client.UseDefaultCredentials = true;
                    client.Credentials = new System.Net.NetworkCredential("test", "test");
                    client.Send(msg);

                    DbConnection db = new DbConnection();
                    db.setDaliyMailCheck(mailTo, "Y", today);
                }
            }
        }

        private string getIntCheck(string data)
        {
            string str = "";
            long number1 = 0;
            bool canConvert = long.TryParse(data, out number1);
            if (canConvert)
            {
                str = string.Format("{0:#,###}", number1).PadLeft(7);
            }
            return str;
        }
    }
}
