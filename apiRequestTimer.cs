using System;
using System.Collections.Generic;
using System.Data;
using System.Timers;

namespace KONEPS_KPC_MailBatch
{
    internal class apiRequestTimer
    {
        private Timer _timer;
        private Int32 _hours = 0;
        private Int32 _runAt = 3;

        private delegate void listBoxAddDelegate(string text);

        private Form1 _form;

        /// <summary>
        /// TimerStart
        /// </summary>
        /// <param name="form"></param>
        public void OnStart(Form1 form)
        {
            _form = form;
            _hours = (24 - (DateTime.Now.Hour + 1)) + _runAt;
            _timer = new Timer();
            _timer.Interval = _hours * 60 * 60 * 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();

            // 1. Api 데이터 수집 및 데이터베이스 저장
            string today = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
            apiRequest api = new apiRequest();
            api.BidPublicInfoService(today);
        }

        /// <summary>
        /// TimerClose
        /// </summary>
        public void OnClose()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Winform Timer 실행 함수
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (_hours != 24)
                {
                    _hours = 24;
                    _timer.Interval = _hours * 60 * 60 * 1000;
                }

                // 1. Api 데이터 수집 및 데이터베이스 저장
                string today = DateTime.Now.AddDays(-1).ToString("yyyyMMdd");
                apiRequest api = new apiRequest();
                api.BidPublicInfoService(today);

                // 2. 사용자 조회
                int mailSendCount = 0;
                List<DbSelectModel> modelList = new List<DbSelectModel>();
                DbConnection dbcon = new DbConnection();

                DataSet ds = dbcon.ERP_SelectUser();
                dbcon.ERP_USER_to_KPC_USER(ds);
                modelList = dbcon.SelectUser();
                // 3. 사용자에 대한 키워드 조회 키워드 있는 사용자 만큼 Count
                for (int z = 0; z < modelList.Count; z++)
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        // 4.사용자 키워드 프로시저 검색
                        dt = dbcon.getKONEPS_KeywordList(today, modelList[z].keyword);

                        DataTable dtSub = new DataTable();
                        dtSub = dbcon.getKONEPS_SUB_KeywordList(today, modelList[z].keyword);
                        // 5.검색된 데이터 Mail폼 만들기
                        // 6.메일 발송
                        if (dt.Rows.Count == 0)
                        {
                            if (dtSub.Rows.Count != 0)
                            {
                                mailSendCount++;

                                mailForm mail = new mailForm();
                                mail.SendMail(mail.BidPublicInfoServiceMailForm(dt, dtSub, modelList[z].keyword, today), modelList[z].email, today);
                            }
                            else
                            {
                                DbConnection db = new DbConnection();
                                db.setDaliyMailCheck(modelList[z].email, "N", today);
                            }
                        }
                        else
                        {
                            mailSendCount++;

                            mailForm mail = new mailForm();
                            mail.SendMail(mail.BidPublicInfoServiceMailForm(dt, dtSub, modelList[z].keyword, today), modelList[z].email, today);
                        }
                    }
                    catch (Exception)
                    {
                        DbConnection db = new DbConnection();
                        db.setDaliyMailCheck(modelList[z].email, "N", today);
                    }
                }

                SetText("▶ (" + mailSendCount + ") 메일 발송 완료" + DateTime.Now);
                mailForm mailResult = new mailForm();
                mailResult.SendMail("▶ (전체 : " + modelList.Count + "중, " + mailSendCount + ") 메일 발송 완료" + DateTime.Now, "test", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                mailResult.SendMail("▶ (전체 : " + modelList.Count + "중, " + mailSendCount + ") 메일 발송 완료" + DateTime.Now, "test", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
                SetText("▶ 메일 발송 실패 " + DateTime.Now);

                mailForm mail = new mailForm();
                mail.SendMail(ex.Message, "test", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                mail.SendMail(ex.Message, "test", DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                throw;
            }
        }

        /// <summary>
        /// Timer 쓰레드로 값 변경 실패시 처리를 위한 Delegate
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            if (_form.InvokeRequired)
            {
                listBoxAddDelegate d = new listBoxAddDelegate(SetText);
                _form.Invoke(d, new object[] { text });
            }
            else
            {
                _form.lbResult.Items.Add(text);
            }
        }
    }
}
