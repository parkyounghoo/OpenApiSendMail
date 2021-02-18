namespace KONEPS_KPC_MailBatch
{
    class KONEPSModel
    {
        public string ntceKindNm { get; set; } //분류
        public string bidNtceDtlUrl { get; set; } //공고명 Url
        public string cntrctCnclsMthdNm { get; set; } //계약방법
        public string taskName { get; set; } //업무 명

        public string bidNtceNm { get; set; } //공고명
        public string bidNtceNo { get; set; } //입찰 공고번호
        public string bidNtceOrd { get; set; } //입찰 공고차수
        public string presmptPrce { get; set; } //추정가격
        public string d2bMngBssamt { get; set; } //기초금액
        public string bidNtceDt { get; set; } //공고일시
        public string opengDt { get; set; } //개찰일시
        public string bidBeginDt { get; set; } //입찰개시일시
        public string bidClseDt { get; set; } //입찰마감일시
        public string ntceInsttNm { get; set; } //공고기관
        public string dminsttNm { get; set; } //수요기관
        public string ntceInsttOfclNm { get; set; } //담당자
        public string ntceInsttOfclTelNo { get; set; } //담당자 번호
        public string asignBdgtAmt { get; set; } //배정예산 금액
    }

    class KONEPSModel_Sub
    {
        public string prdctClsfcNoNm { get; set; } //품목명,공고명
        public string bfSpecRgstNo { get; set; } //사전규격등록번호
        public string asignBdgtAmt { get; set; } //배정예산금액
        public string rcptDt { get; set; } //접수일시
        public string opninRgstClseDt { get; set; } //의견등록마감일시
        public string orderInsttNm { get; set; } //발주기관
        public string rlDminsttNm { get; set; } //실수요기관
        public string ofclNm { get; set; } //담당자
        public string ofclTelNo { get; set; } //담당자번호
        public string taskName { get; set; }
        public string bidNtceDtlUrl { get; set; }
    }

    //개찰결과 목록
    class OpengResultListInfoServc
    {
        //업무
        public string taskName { get; set; }
        //입찰공고번호 - 차수
        public string bidNtceNo { get; set; }
        public string bidNtceOrd { get; set; }
        //입찰분류번호
        public string bidClsfcNo { get; set; }
        //재입찰번호
        public string rbidNo { get; set; }
        //공고명
        public string bidNtceNm { get; set; }
        //수요기관
        public string dminsttNm { get; set; }
        //개찰일시
        public string opengDt { get; set; }
        //참가수
        public string prtcptCnum { get; set; }
        //낙찰예정자 경우 업체명, 사업자번호, 대표자명, 투찰금액, 투찰율을 보여줌. ^
        public string opengCorpInfo { get; set; }
        //업체명
        public string enterName { get; set; }
        //투찰금액
        public string bddprDlamt { get; set; }
        //투찰률
        public string bddprRT { get; set; }
        //진행상황
        public string progrsDivCdNm { get; set; }
    }

    //개찰완료 목록
    class OpengResultListInfoOpengCompt
    {
        //입찰공고번호
        public string bidNtceNo { get; set; }
        //차수
        public string bidNtceOrd { get; set; }
        //입찰분류번호
        public string bidClsfcNo { get; set; }
        //재입찰
        public string rbidNo { get; set; }
        //개찰순위
        public string opengRank { get; set; }
        //사업자등록번호
        public string prcbdrBizno { get; set; }
        //업체명
        public string prcbdrNm { get; set; }
        //대표자명
        public string prcbdrCeoNm { get; set; }
        //입찰금액(원)
        public string bidprcAmt { get; set; }
        //투찰률(%)
        public string bidprcrt { get; set; }
        //추첨번호
        public string drwtNo1 { get; set; }
        public string drwtNo2 { get; set; }
        //투찰일시
        public string bidprcDt { get; set; }
        //비고
        public string rmrk { get; set; }
        //사업자 링크 http://www.g2b.go.kr:8101/ep/result/selectTechEvalScore.do?bidno=입찰공고번호&bidseq=입찰공고차수&bidcate=입찰분류번호&bizRegNo=투찰업체사업자등록번호
        public string prcbdrBiznoLink { get; set; }
    }

    //낙찰결과 목록
    class ScsbidListSttusServc
    {
        //업무
        public string taskName { get; set; }
        //입찰공고번호
        public string bidNtceNo { get; set; }
        //차수
        public string bidNtceOrd { get; set; }
        //입찰분류번호
        public string bidClsfcNo { get; set; }
        //재입찰
        public string rbidNo { get; set; }
        //공고명
        public string bidNtceNm { get; set; }
        //수요기관
        public string dminsttNm { get; set; }
        //개찰일시
        public string rlOpengDt { get; set; }
        //참가수
        public string prtcptCnum { get; set; }
        //낙찰자
        public string bidwinnrNm { get; set; }
        //낙찰금액(원)
        public string sucsfbidAmt { get; set; }
        //낙찰률(%)
        public string sucsfbidRate { get; set; }
    }
    class DbSelectModel
    {
        public string email { get; set; } //사용자 email
        public string keyword { get; set; } //검색할 keyword
    }

    class TechEvalScore
    {
        //공고번호
        public string bidNtceNo { get; set; }
        //차수
        public string bidNtceOrd { get; set; }
        //분류번호
        public string bidClsfcNo { get; set; }
        //재입찰
        public string rbidNo { get; set; }
        //사업자등록번호
        public string prcbdrBizno { get; set; }
        //사업자명
        public string prcbdrBizNm { get; set; }
        //입찰가격점수
        public string bidPricePoint { get; set; }
        //기술평가점수
        public string tchQvlnPoint { get; set; }
        //종합평점
        public string gnrlzQvlnPoint { get; set; }
        //공개위치
        public string openLocation { get; set; }
        //공개일시
        public string openDate { get; set; }
    }
}
