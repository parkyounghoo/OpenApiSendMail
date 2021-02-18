using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace KONEPS_KPC_MailBatch
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
            mailForm.Append("<font style='font-size:12px; font-family:'Malgun Gothic''>안녕하세요</font>");
            mailForm.Append("<br />");
            mailForm.Append("<font style='font-size:12px; font-family:'Malgun Gothic''>디지털혁신센터 최민석입니다.</font>");
            mailForm.Append("<br />");
            mailForm.Append("<br />");
            mailForm.Append("<font style='font-size:12px; font-family:'Malgun Gothic''>금일 나라장터 입찰정보 전달합니다.</font>");
            mailForm.Append("<br />");
            mailForm.Append("<font style='font-size:12px; font-family:'Malgun Gothic''>관련 내용 업무에 참고하시기 바랍니다.</font>");
            mailForm.Append("<br />");
            mailForm.Append("<font style='font-size:12px; font-family:'Malgun Gothic''>( * 입찰 정보가 없는 경우 메일 발송을 하지 않습니다.)</font>");
            mailForm.Append("<br /><br />");
            mailForm.Append("<font style='font-size:12px; color:blue; font-weight:bold;font-family:'Malgun Gothic''>* 나라장터 등록일자 : </font>");
            mailForm.Append("<font style='font-size:12px; font-weight:bold;font-family:'Malgun Gothic''>" + DateTime.Now.AddDays(-1).ToString("yyyy.MM.dd") + "</font>");
            mailForm.Append("<br /><br />");
            mailForm.Append("<font style='font-size:12px; color: blue; font-weight:bold;font-family:'Malgun Gothic''>* 검색키워드 : </font>");
            mailForm.Append("<font style='font-size:12px; font-weight: bold;font-family:'Malgun Gothic''>" + keyword + "</font>");
            mailForm.Append("<br />");
            mailForm.Append("&nbsp;&nbsp;<font style='font-size:12px; font-family:'Malgun Gothic''>- 입찰정보 키워드 변경은 ERP (일반업무 >> 나라장터입찰정보)에서 수정 가능합니다.</font>");
            mailForm.Append("<div style='margin: 20px auto 0px; width: 800px;'>");
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

            #region mailForm

            //mailForm.Append(@"<html>");

            //#region table style

            //mailForm.Append("<head>");
            ////mailForm.Append("<style type='text/css'>");
            ////mailForm.Append("table.type11 { border-collapse: separate; border-spacing: 1px; text-align: center; line-height: 1.5; margin: 20px 10px; }");
            ////mailForm.Append("table.type11 th { padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f; }");
            ////mailForm.Append("table.type11 td { padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee; }");
            ////mailForm.Append("</style>");
            //mailForm.Append("</head>");

            //#endregion table style

            //mailForm.Append("   <body>");
            //mailForm.Append("       안녕하세요 KPC입니다. 금일 나라장터 입찰공고 내용입니다.");
            //mailForm.Append("       <br />");
            //mailForm.Append("       참조 부탁드립니다.");
            //mailForm.Append("       <br /><br />");
            //mailForm.Append("       <label style='color: blue; font-weight:bold'>[검색일자] : </label>");
            //mailForm.Append("<label style='font-weight: bold;' >" + DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + "</label>");
            //mailForm.Append("       <br />");
            //mailForm.Append("       <label style='color: blue; font-weight:bold'>[검색어] : </label>");
            //mailForm.Append("<label style='font-weight: bold;' >" + keyword + "</label>");
            //mailForm.Append("       <table style= 'border-collapse: separate; font-size:11px; border-spacing: 1px; text-align: center; line-height: 1.5; margin: 20px 10px;'>");
            //mailForm.Append("           <thead>");
            //mailForm.Append("               <tr>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;width:25px'>업무</th>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;'>공고번호-차수</th>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;'>공고명</th>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;'>공고기관</th>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;'>계약방법</th>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;width:50px'>분류</th>");
            //mailForm.Append("                   <th style='padding: 10px; font-weight: bold; vertical-align: top; color: #fff; background: #0c588f;width:60px'>입찰개시일시</br>(입찰마감일시)</th>");
            //mailForm.Append("               </tr>");
            //mailForm.Append("           </thead>");
            //mailForm.Append("           <tbody>");

            //#region api Data To MailForm

            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    DataRow dr = dt.Rows[i];
            //    mailForm.Append("<tr>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'>" + dr["taskName"] + "</td>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'><a href=" + dr["bidNtceDtlUrl"] + " target='_blank'>" + dr["bidNtceNo"] + "-" + dr["bidNtceOrd"] + "</a></td>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'><a href=" + dr["bidNtceDtlUrl"] + " target='_blank'>");

            //    #region keyword Mark

            //    string mailFormComplete = dr["bidNtceNm"].ToString();
            //    string[] keywordSplit = keyword.Split('/');
            //    for (int z = 0; z < keywordSplit.Count(); z++)
            //    {
            //        mailFormComplete = mailFormComplete.Replace(keywordSplit[z], "<label style='color: white; background-color:red'>" + keywordSplit[z] + "</label>");
            //    }
            //    mailForm.Append(mailFormComplete);

            //    #endregion keyword Mark

            //    mailForm.Append("</a></td>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'>" + dr["ntceInsttNm"] + "</td>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'>" + dr["cntrctCnclsMthdNm"] + "</td>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'>" + dr["ntceKindNm"] + "</td>");
            //    mailForm.Append("<td style='padding: 10px; vertical-align: top; border-bottom: 1px solid #ccc; background: #eee;'>" + dr["bidBeginDt"] + "</br>(" + dr["bidClseDt"] + ")</td>");
            //    mailForm.Append("</tr>");
            //}

            //#endregion api Data To MailForm

            //mailForm.Append("           </tbody>");
            //mailForm.Append("       </table>");
            //mailForm.Append("       <br />");
            //mailForm.Append("       감사합니다.");
            //mailForm.Append("   </body>");
            //mailForm.Append("</html>");

            #endregion mailForm

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
                MailMessage msg = new MailMessage("account@kpc.or.kr", mailTo);
                msg.Subject = "[디지털혁신센터] " + DateTime.Now.ToString("MM.dd") + " 나라장터 입찰정보(사전규격/입찰)";
                msg.IsBodyHtml = true;
                msg.Body = mailForm;

                using (SmtpClient client = new SmtpClient("124.111.37.73", 25))
                {
                    //client.EnableSsl = true;
                    //client.UseDefaultCredentials = true;
                    client.Credentials = new System.Net.NetworkCredential("account@kpc.or.kr", "$$$kpc1004");
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