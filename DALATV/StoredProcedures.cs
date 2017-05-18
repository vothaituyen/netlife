using System.Data;
using System;
using ATVEntity;
namespace DALATV
{
    /// <summary>
    /// 
    /// </summary>
    public class StoredProcedures : StoredProcedures_Base
    {

        private MainDB _db;

        public StoredProcedures(MainDB db)
            : base(db)
        {
            _db = db;
        }

        #region NewsPublished


        public DataTable NP_DanhSachTinCount(int Cat_ID)
        {
            return new DataTable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="News_ID"></param>
        /// <returns></returns>
        public DataTable NP_TinChiTiet(long News_ID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_TinChiTiet", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cat_ID"></param>
        /// <param name="News_ID"></param>
        /// <param name="Top"></param>
        /// <returns></returns>
        public DataTable NP_TinXuatBanTruoc(int Cat_ID, int News_ID, int Top)
        {
            return new DataTable();
        }


        /// <summary>
        /// Tra lại top N tin xuat bản sau tin đã đưa
        /// </summary>
        /// <param name="Cat_ID">ID chuyen muc</param>
        /// <param name="News_ID">Tin da dang</param>
        /// <param name="Top">So luong tin</param>
        /// <returns></returns>
        public DataTable NP_TinXuatBanSau(int Cat_ID, int News_ID, int Top)
        {
            return new DataTable();
        }

        /// <summary>
        /// Chon top N bai moi nhat theo Cat
        /// </summary>
        /// <param name="cat_id">Category ID</param>
        /// <param name="top">So luong tin</param>
        /// <returns></returns>
        public DataTable NP_SelectTopHotByCat(int cat_id, int top, int news_mode)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_NoiBatChuyenMuc", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "News_Mode", DbType.Int32, news_mode);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_SelectHoaHauTopHotByCat(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_HoaHauNoiBatChuyenMuc", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        public DataTable NP_Tin_Lien_Quan(string News_ID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Tin_Lien_Quan", true);
            _db.AddParameter(cmd, "News_ID", DbType.String, News_ID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_Select_Tin_Khac(int cat_id, long News_ID, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_TinXuatBanSau", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        //  Add new store proc to get "Tin Cung Chuyen Muc"
        public DataTable NP_Select_Tin_Cung_Chuyen_Muc(int cat_id, long News_ID, int top, string relatedNewsId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_TinCungChuyenMuc", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "Related_News_Id", DbType.String, relatedNewsId); //  ADD 20160525, loc tin cung chuyen muc khong trung voi tin lien quan
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        #endregion

        #region Category
        public DataTable GetCategoryByID(int catId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_SelectCategory", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, catId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        #endregion

        public DataTable GetCommentByNewsIdAndPage(long news_id, int pageIndex, int pageSize)
        {
            IDbCommand cmd = _db.CreateCommand("GetCommentByNewsIDAndPage", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, news_id);
            _db.AddParameter(cmd, "PageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "PageSize", DbType.Int32, pageSize);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        public DataTable Comment_GetByNewsID(long news_id)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Comment_GetByNewsID", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, news_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public void Comment_Insert(CommentEntity ce)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Comment_Insert", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, ce.News_ID);
            _db.AddParameter(cmd, "Comment_User", DbType.String, ce.Comment_User);
            _db.AddParameter(cmd, "Comment_Content", DbType.String, ce.Comment_Content);
            _db.AddParameter(cmd, "Comment_Date", DbType.DateTime, ce.Comment_Date);
            _db.AddParameter(cmd, "Comment_Email", DbType.String, ce.Comment_Email);
            _db.AddParameter(cmd, "Avatar", DbType.String, ce.Avatar);
            _db.AddParameter(cmd, "ParentId", DbType.Int64, ce.CommentParent);
            DataTable table = _db.CreateDataTable(cmd);
        }

        public void CommentLike(Int64 commentId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Comment_Like", true);
            _db.AddParameter(cmd, "CommentId", DbType.Int64, commentId);
            DataTable table = _db.CreateDataTable(cmd);
        }

        #region Tin Don
        public void Tin_Don_Insert(TinDonEntity tde)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Comment_Insert", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int32, tde.TinDon_ID);
            DataTable table = _db.CreateDataTable(cmd);
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion

        public DataTable Select_Tin_Don(int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Select_Top_Tin_Don", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Cine_GetById(int CineID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Cine_SelectItem", true);
            _db.AddParameter(cmd, "CineID", DbType.Int32, CineID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;

        }

        #region QA
        /// <summary>
        /// Trả lại danh sách QA theo Top N tin
        /// </summary>
        /// <param name="catId">Loại tin</param>
        /// <param name="top">Số lượng</param>
        /// <returns></returns>
        public DataTable QA_SelectItem(int catId, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_QA_SelectItem", true);
            _db.AddParameter(cmd, "CatId", DbType.Int32, catId);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        /// <summary>
        /// Lây danh sách
        /// </summary>
        /// <param name="catId">Chuyên mục</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="pageIndex">Page Index</param>
        /// <returns></returns>
        public DataTable QA_SelectItem(int catId, int pageSize, int pageIndex)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_QA_SelectListItem", true);
            _db.AddParameter(cmd, "CatId", DbType.Int32, catId);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, pageIndex);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        public DataTable QA_CountItem(int catId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_QA_CountItem", true);
            _db.AddParameter(cmd, "CatId", DbType.Int32, catId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable QA_InsertItem(QAEntity qa)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_QA_InsertItem", true);
            _db.AddParameter(cmd, "CatId", DbType.Int32, qa.CATID);
            _db.AddParameter(cmd, "Name", DbType.String, qa.NAME);
            _db.AddParameter(cmd, "Email", DbType.String, qa.EMAIL);
            _db.AddParameter(cmd, "CreatedDate", DbType.Date, qa.CREATEDDATE);
            _db.AddParameter(cmd, "Question", DbType.String, qa.QUESTION);
            _db.AddParameter(cmd, "Status", DbType.Int32, qa.STATUS);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable QA_InsertItemNhatKy(QAEntity qa)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_QA_InsertNhatKy", true);
            _db.AddParameter(cmd, "CatId", DbType.Int32, qa.CATID);
            _db.AddParameter(cmd, "Name", DbType.String, qa.NAME);
            _db.AddParameter(cmd, "Email", DbType.String, qa.EMAIL);
            _db.AddParameter(cmd, "CreatedDate", DbType.Date, qa.CREATEDDATE);
            _db.AddParameter(cmd, "Question", DbType.String, qa.QUESTION);
            _db.AddParameter(cmd, "Status", DbType.Int32, qa.STATUS);
            _db.AddParameter(cmd, "Answer", DbType.String, qa.Answer);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        #endregion

        public DataTable GetTopCine(int top, int type)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Cine_SelectListByTop", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "Type", DbType.Int32, type);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        #region Flash Home
        public DataTable FlashHome_GetAll()
        {
            IDbCommand cmd = _db.CreateCommand("Sao_HomeFlashPublished_SelectAlltem", true);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        #endregion

        public DataTable NP_Select_Tin_Tieu_Diem(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Select_Tin_Tieu_Diem", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Get_Danh_Sach_Tin(int cat_id, int pageSize, int pageIndex, string news_ids)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Get_Danh_Sach_Tin", true);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_IDs", DbType.String, news_ids);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Get_Danh_Sach_Tin_Hidden(int cat_id, int pageSize, int pageIndex, string news_ids)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Get_Danh_Sach_Tin_Hidden", true);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_IDs", DbType.String, news_ids);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Get_Danh_Sach_Tin_QA(int cat_id, int pageSize, int pageIndex)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Get_Danh_Sach_Tin_Hoi_Dap", true);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Get_Danh_Sach_Tin_Count(int cat_id, string news_id)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Get_Danh_Sach_Tin_Count", true);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_IDs", DbType.String, news_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Get_Danh_Sach_Tin_Hidden_Count(int cat_id, string news_id)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_Get_Danh_Sach_Tin_Hidden_Count", true);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_IDs", DbType.String, news_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        #region Thread
        public DataTable GetThreadByCat(int Cat_Id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_GetThreadByCat", true);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, Cat_Id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        #endregion

        public DataTable NP_Dong_Su_Kien(int threadId, int pageSize, int pageIndex)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_GetNewsByThreadId", true);
            _db.AddParameter(cmd, "threadId", DbType.Int32, threadId);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, pageIndex);

            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetThreadById(int threadId)
        {

            IDbCommand cmd = _db.CreateCommand("cms_NewsThread_SelectOne", true);
            _db.AddParameter(cmd, "Thread_ID", DbType.Int32, threadId);

            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetCategoryByParent(int parentID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_GetCategoryByParent", true);
            _db.AddParameter(cmd, "ParentID", DbType.Int32, parentID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetCategoryByParentRemoveHidden(int parentID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_GetCategoryByParentWithHidden", true);
            _db.AddParameter(cmd, "ParentID", DbType.Int32, parentID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        public DataTable GetCategoryByParentAndTop(int parentID, int Top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_GetCategoryByParentAndTop", true);
            _db.AddParameter(cmd, "ParentID", DbType.Int32, parentID);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_SelectTopQnA(int cat_id, int top, int news_mode)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_NoiBatHoiDap", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "News_Mode", DbType.Int32, news_mode);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable SinhNhatSao_Select(DateTime dt)
        {
            IDbCommand cmd = _db.CreateCommand("SAO_SelectSaoByDate", true);
            _db.AddParameter(cmd, "date", DbType.DateTime, dt);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetTopAlbum(int top)
        {
            IDbCommand cmd = _db.CreateCommand("SAO_AlbumSelectTop", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetTopNewestCommentByCatID(int Top, int CatID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_SelectTopNewsestCommentByCategory", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            _db.AddParameter(cmd, "CatID", DbType.Int32, CatID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Sao_Hit_Cau_Hoi(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Hit_Cau_Hoi", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, cat_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        public DataTable NP_SelectTopByCat(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("NP_SelectTopHotByCat", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_Select_Top_Home(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("NP_Select_Top_Home", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        public DataTable NP_Select_Top_HomeCatNotHome(int top)
        {
            IDbCommand cmd = _db.CreateCommand("NP_Select_Top_Home_CatNotHome", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        public DataTable NP_SelectTopTinDon(int cat_id, int top, bool ishome)
        {
            IDbCommand cmd = _db.CreateCommand("NP_SelectTopTinDon", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "ishome", DbType.Boolean, ishome);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        public DataTable NP_Select_Top_Cat(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("NP_Select_Top_Cat", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_Select_Top_Cat_None_Child(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("NP_Select_Top_Cat_None_Child", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable QA_DoAdvanceSearch(QASearchEntity qae)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_AdvanceSearch", true);
            _db.AddParameter(cmd, "key", DbType.String, qae.KeySearch);
            _db.AddParameter(cmd, "cat", DbType.Int32, qae.Cat);
            _db.AddParameter(cmd, "type", DbType.Int32, qae.Type != 0 ? 1 : 0);
            _db.AddParameter(cmd, "order", DbType.Int32, qae.Order != 0 ? 1 : 0);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, qae.PageIndex > 0 ? qae.PageIndex : 1);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, qae.PageSize > 0 ? qae.PageSize : 10);
            _db.AddParameter(cmd, "edate", DbType.DateTime, qae.EDate);
            _db.AddParameter(cmd, "fdate", DbType.DateTime, qae.FDate);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable News_DoAdvanceSearch(QASearchEntity qae)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_AdvanceSearchNews", true);
            _db.AddParameter(cmd, "key", DbType.String, qae.KeySearch.Replace(" ", "+"));
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, qae.PageIndex > 0 ? qae.PageIndex : 1);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, qae.PageSize > 0 ? qae.PageSize : 10);
            _db.AddParameter(cmd, "edate", DbType.DateTime, qae.EDate);
            _db.AddParameter(cmd, "fdate", DbType.DateTime, qae.FDate);
            _db.AddParameter(cmd, "catId", DbType.Int32, qae.Cat);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        public DataTable NP_Dong_Su_Kien_Count(int threadId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_GetNewsByThreadId_Count", true);
            _db.AddParameter(cmd, "threadId", DbType.Int32, threadId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable TTOL_Dong_Su_Kien_All_Count()
        {
            IDbCommand cmd = _db.CreateCommand("TTOL_GetNewsThreadId_Count", true);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_SelectTin_Moi_Nhat_ByCat(int cat_id, int top, string news_id)
        {
            IDbCommand cmd = _db.CreateCommand("NP_SelectTin_Moi_Nhat_ByCat_hidden", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            _db.AddParameter(cmd, "NewsID", DbType.String, news_id.TrimEnd(','));
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        //public DataTable Get_TinDocNhieuNhat(int top, string news_id)
        //{
        //    IDbCommand cmd = _db.CreateCommand("Get_TinDocNhieuNhat", true);
        //    _db.AddParameter(cmd, "Top", DbType.Int32, top);
        //    _db.AddParameter(cmd, "NewsID", DbType.String, news_id.TrimEnd(','));
        //    DataTable table = _db.CreateCommand(cmd);
        //    return table;
        //}

        public DataTable Get_SearchByDate(int cat_id, DateTime dt)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_SearchByDate", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "date", DbType.DateTime, dt);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetMetaByID(int cat_id)
        {
            IDbCommand cmd = _db.CreateCommand("GetMetaByID", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable MapID(string id)
        {
            IDbCommand cmd = _db.CreateCommand("SAO_Map_ID", true);
            _db.AddParameter(cmd, "id", DbType.String, id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetAdvItemById(int zoneId, int catId)
        {
            IDbCommand cmd = _db.CreateCommand("GetAdvItemById", true);
            _db.AddParameter(cmd, "zoneId", DbType.Int32, zoneId);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetAdvByZoneId(int zoneId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DataTable GetZoneByCatId(int catId)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public DataTable Sao_SelectVoteItemByVoteId(int voteId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_SelectVoteItemByVoteId", true);
            _db.AddParameter(cmd, "voteId", DbType.Int32, voteId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Sao_SelectVoteById(int voteId, int catId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_SelectVoteById", true);
            _db.AddParameter(cmd, "Id", DbType.Int32, voteId);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        public DataTable Sao_SelectVoteById(int voteId)
        {
            IDbCommand cmd = _db.CreateCommand("v2_fe_VoteByVoteId", true);
            _db.AddParameter(cmd, "Vote_ID", DbType.Int32, voteId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }



        public DataTable Sao_UpdateVoteById(int voteItemId, string other, int voteId)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_UpdateVoteById", true);
            _db.AddParameter(cmd, "voteItemId", DbType.Int32, voteItemId);
            _db.AddParameter(cmd, "voteId", DbType.Int32, voteId);
            _db.AddParameter(cmd, "other", DbType.String, other);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public void QuaTang(string hoten, string ngaysinh, string email, string diachi, string cmt, string dienthoai)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Dangky", true);
            _db.AddParameter(cmd, "hoten", DbType.String, hoten);
            _db.AddParameter(cmd, "ngaysinh ", DbType.String, ngaysinh);
            _db.AddParameter(cmd, "email", DbType.String, email);
            _db.AddParameter(cmd, "diachi", DbType.String, diachi);
            _db.AddParameter(cmd, "cmt", DbType.String, cmt);
            _db.AddParameter(cmd, "dianthoai", DbType.String, dienthoai);
            DataTable table = _db.CreateDataTable(cmd);
        }

        public DataTable NP_Select_Tin_Moi(object cat_id, object News_ID, object top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_TinXuatBanTruoc", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, cat_id);
            _db.AddParameter(cmd, "News_ID ", DbType.Int64, News_ID);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable CategoryBuildTree()
        {
            IDbCommand cmd = _db.CreateCommand("SAO_BuildTreeCategory", true);
            DataTable table = _db.CreateDataTable(cmd);
            return table;

        }

        public DataTable News_DoAdvanceSearchCount(QASearchEntity qae)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_AdvanceSearchNewsCount", true);
            _db.AddParameter(cmd, "key", DbType.String, qae.KeySearch.Replace(" ", "+"));
            _db.AddParameter(cmd, "edate", DbType.DateTime, qae.EDate);
            _db.AddParameter(cmd, "fdate", DbType.DateTime, qae.FDate);
            _db.AddParameter(cmd, "catId", DbType.Int32, qae.Cat);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable GetListThreadByCat(int Cat_Id, int pageSize, int pageIndex)
        {
            IDbCommand cmd = _db.CreateCommand("GetListThreadByCat", true);
            _db.AddParameter(cmd, "Cat_Id", DbType.Int32, Cat_Id);
            _db.AddParameter(cmd, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "pageIndex", DbType.Int32, pageIndex);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Sao_DamLuanChild(long news_Id)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_SelectListChildNews", true);
            _db.AddParameter(cmd, "NewsId", DbType.Int64, news_Id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Sao_GetChannel()
        {
            IDbCommand cmd = _db.CreateCommand("Sao_GetAllKenhTruyenHinh", true);
            DataTable table = _db.CreateDataTable(cmd);
            return table;

        }

        public DataTable Sao_GetLinkTheoNgay(int channelID, DateTime date)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_GetLichTruyenHinhByDate", true);
            _db.AddParameter(cmd, "channelID", DbType.Int32, channelID);
            _db.AddParameter(cmd, "date", DbType.DateTime, date);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_TinChiTietBaiCon(Int64 News_ID)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_NP_TinChiTietBaiCon", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Sao_GetMeidaObject_ByNews(long News_Id)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_GetMeidaObject_ByNews", true);
            _db.AddParameter(cmd, "newsId", DbType.Int64, News_Id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_HoaHauVote(int voteId, int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_Top10HoaHauVote", true);
            _db.AddParameter(cmd, "voteId", DbType.Int64, voteId);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_Tin_Moi_Trong_Ngay(int top)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_TinMoiTrongNgay", true);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        //thuynt
        public DataTable NP_Tin_Hot(int top, int catid)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_TinHot", true);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            _db.AddParameter(cmd, "cat", DbType.Int32, catid);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        //ducdm add news
        //Get New by newsmode
        public DataTable NP_Tin_Nong(int catid, int newsMode, int top)
        {
            IDbCommand cmd = _db.CreateCommand("cntt_ListNewsByNewsMode", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, catid);
            _db.AddParameter(cmd, "News_Mode", DbType.Int32, newsMode);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        //ducdm add news
        //Get New by newsmode 2
        public DataTable GetListNewbyNewsMode2(int catid, int newsMode, int newsmode2, int top)
        {
            IDbCommand cmd = _db.CreateCommand("cntt_ListNewsByNewsMode2", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, catid);
            _db.AddParameter(cmd, "News_Mode", DbType.Int32, newsMode);
            _db.AddParameter(cmd, "News_Mode2", DbType.Int32, newsmode2);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        //ducdm add news
        //Get New by newsmode 3
        public DataTable GetListNewbyNewsMode3(int catid, int newsMode, int newsmode2, int newsMode3, int top)
        {
            IDbCommand cmd = _db.CreateCommand("cntt_ListNewsByNewsMode3", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, catid);
            _db.AddParameter(cmd, "News_Mode", DbType.Int32, newsMode);
            _db.AddParameter(cmd, "News_Mode2", DbType.Int32, newsmode2);
            _db.AddParameter(cmd, "News_Mode3", DbType.Int32, newsMode3);
            _db.AddParameter(cmd, "Top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_Xem_Nhieu_Nhat(int top,int cat)
        {
            IDbCommand cmd = _db.CreateCommand("Sao_XemNhieuNhat", true);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            _db.AddParameter(cmd, "cat", DbType.Int32, cat);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_Nhat_Ky_Nhieu_Comment(int top, int catId)
        {
            IDbCommand cmd = _db.CreateCommand("NP_GetNewsByComment_ASC", true);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable NP_NhatKyNoiBat(int cat_id, int top)
        {
            IDbCommand cmd = _db.CreateCommand("NP_NhatKyNoiBat", true);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            _db.AddParameter(cmd, "catId", DbType.Int32, cat_id);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        public DataTable Yahoo_Push_News(string catid)
        {
            IDbCommand command = this._db.CreateCommand("Yahoo_Push_News", true);
            _db.AddParameter(command, "catId", DbType.String, catid);
            return this._db.CreateDataTable(command);
        }



        public DataTable SearchFulltext(string key, int pageIndex, int pageSize,int catId)
        {
            IDbCommand command = this._db.CreateCommand("NHN_Search_News_FullTextSearch", true);
            _db.AddParameter(command, "KeySearch", DbType.String, key);
            _db.AddParameter(command, "pageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(command, "pageSize", DbType.Int32, pageSize);
            _db.AddParameter(command, "Cat_ID", DbType.Int32, catId);
            
            return this._db.CreateDataTable(command);
        }

        public DataTable SearchFulltextCount(string key, int pageSize,int catId)
        {
            IDbCommand command = this._db.CreateCommand("NHN_Search_News_FullTextSearch_Count", true);
            _db.AddParameter(command, "KeySearch", DbType.String, key);
            _db.AddParameter(command, "Cat_ID", DbType.Int32, catId);
            return this._db.CreateDataTable(command);
        }

        public void AdvUpdateView(int advId)
        {
            IDbCommand command = this._db.CreateCommand("AdvUpdateView", true);
            _db.AddParameter(command, "AdvId", DbType.Int32, advId);

            this._db.CreateDataTable(command);
        }

        public DataTable GetTinForGoogleNews(int top)
        {
            IDbCommand command = this._db.CreateCommand("GetTinForGoogleNews", true);
            _db.AddParameter(command, "top", DbType.Int32, top);
            return this._db.CreateDataTable(command);
        }


        public void WapTransaction_Insert(string ReturnParam, string Phone, int ServiceType, int CategoryId, Int64 ArticleId, int Price)
        {
            IDbCommand command = this._db.CreateCommand("WapTransaction_Insert", true);
            _db.AddParameter(command, "ReturnParam", DbType.String, ReturnParam);
            _db.AddParameter(command, "Phone", DbType.String, Phone);
            _db.AddParameter(command, "ServiceType", DbType.Int32, ServiceType);
            _db.AddParameter(command, "CategoryId", DbType.Int32, CategoryId);
            _db.AddParameter(command, "ArticleId", DbType.Int64, ArticleId);
            _db.AddParameter(command, "Price", DbType.Int32, Price);
            this._db.CreateDataTable(command);
        }

        public DataTable UsersGetOne(string email)
        {
            return UsersGetOne(email, string.Empty);
        }

        public void UsersUpdate(UserEntity userEntity)
        {
            IDbCommand command = this._db.CreateCommand("UsersUpdate", true);
            _db.AddParameter(command, "Email", DbType.String, userEntity.Email);
            _db.AddParameter(command, "Phone", DbType.String, userEntity.Phone);
            _db.AddParameter(command, "Password", DbType.String, userEntity.Password);
            _db.AddParameter(command, "ResetPass", DbType.Guid, userEntity.ResetPass);
            _db.AddParameter(command, "IsActive", DbType.Boolean, userEntity.IsActive);
            _db.AddParameter(command, "FullName", DbType.String, userEntity.FullName);
            _db.AddParameter(command, "Created", DbType.DateTime, userEntity.Created);
            _db.AddParameter(command, "Address", DbType.String, userEntity.Address);
            this._db.CreateDataTable(command);
        }

        public void UsersInsert(UserEntity userEntity)
        {
            IDbCommand command = this._db.CreateCommand("UsersInsert", true);
            _db.AddParameter(command, "Email", DbType.String, userEntity.Email);
            _db.AddParameter(command, "Phone", DbType.String, userEntity.Phone);
            _db.AddParameter(command, "Password", DbType.String, userEntity.Password);
            _db.AddParameter(command, "ResetPass", DbType.Guid, userEntity.ResetPass);
            _db.AddParameter(command, "IsActive", DbType.Boolean, userEntity.IsActive);
            _db.AddParameter(command, "FullName", DbType.String, userEntity.FullName);
            _db.AddParameter(command, "Created", DbType.DateTime, userEntity.Created);
            _db.AddParameter(command, "Address", DbType.String, userEntity.Address);
            this._db.CreateDataTable(command);
        }

        public DataTable UsersGetAll(int pageSize, int pageIndex)
        {
            throw new NotImplementedException();
        }

        public DataTable UsersGetOne(string email, string phone)
        {
            IDbCommand command = this._db.CreateCommand("UsersGetOne", true);
            _db.AddParameter(command, "email", DbType.String, email);
            _db.AddParameter(command, "phone", DbType.String, phone);
            return this._db.CreateDataTable(command);
        }

        public DataTable GetOneByPhone(string phone)
        {
            IDbCommand command = this._db.CreateCommand("UsersGetOneByPhone", true);
            _db.AddParameter(command, "phone", DbType.String, phone);
            return this._db.CreateDataTable(command);
        }

        public DataTable Payment(string Email, int Price, DateTime dateTime)
        {
            IDbCommand command = this._db.CreateCommand("UsersPaymentInsert", true);
            _db.AddParameter(command, "Email", DbType.String, Email);
            _db.AddParameter(command, "Price", DbType.Int32, Price);
            _db.AddParameter(command, "PayDate", DbType.DateTime, dateTime);
            return this._db.CreateDataTable(command);
        }

        public DataTable GetMediaLienquan(long newsid)
        {
            IDbCommand command = this._db.CreateCommand("VideoInfo_GetByNewsId", true);
            _db.AddParameter(command, "News_ID", DbType.Int64, newsid);
            return this._db.CreateDataTable(command);
        }

        public DataTable VideoInfoCategory(int CategoryId)
        {
            IDbCommand command = this._db.CreateCommand("VideoInfo_GetByCategoryId", true);
            _db.AddParameter(command, "CatId", DbType.Int64, CategoryId);
            return this._db.CreateDataTable(command);
        }

        public DataTable NewsPublished_Get_Top_Home(int cat_id, int top, int topnews)
        {

            IDbCommand command = this._db.CreateCommand("NewsPublished_Get_Top_Home", true);
            _db.AddParameter(command, "cat_id", DbType.Int32, cat_id);
            _db.AddParameter(command, "topUp", DbType.Int32, top);
            _db.AddParameter(command, "topDown", DbType.Int32, topnews);
            return this._db.CreateDataTable(command);
        }

        public DataTable NoiBatTrangChu(int top)
        {
            IDbCommand command = this._db.CreateCommand("v2_fe_NoiBatTrangChuInnerUp", true);
            _db.AddParameter(command, "top", DbType.Int32, top);
            return this._db.CreateDataTable(command);
        }

        public DataTable GetListBonBaiNoibat(int top)
        {
            //IDbCommand command = this._db.CreateCommand("TTOL_SelectTopInBonBaiNoiBat", true);
            IDbCommand command = this._db.CreateCommand("NewsPublished_NoiBatTrangChu", true);
            _db.AddParameter(command, "top", DbType.Int32, top);
            return this._db.CreateDataTable(command);
        }

        /// <summary>
        /// Get Thread của trang chủ
        /// </summary>
        /// <param name="Cat_Id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataTable GetListThreadHome(int top)
        {
            IDbCommand cmd = _db.CreateCommand("ttol_GetThreadByTop", true);
            _db.AddParameter(cmd, "top", DbType.Int32, top);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }
        /// <summary>
        /// lấy bài mới nhất theo chuyên mục
        /// </summary>
        /// <param name="cat_id"></param>
        /// <param name="top"></param>
        /// <param name="news_id"></param>
        /// <returns></returns>
        public DataTable GetListNewsByCatAndDate(int Cat_ID, long News_ID, int PageIndex, int PageSize)
        {
            IDbCommand cmd = _db.CreateCommand("TTVHOnline_DanhSachTin", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, Cat_ID);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            _db.AddParameter(cmd, "PageIndex", DbType.Int32, PageIndex);
            _db.AddParameter(cmd, "PageSize", DbType.Int32, PageSize);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }

        /// <summary>
        /// Lấy video theo catid và top
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public DataTable GetVideoByCatAndTop(int Top, int catId)
        {
            IDbCommand cmd = _db.CreateCommand("TTOL_SelectVideoHome", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }

        /// <summary>
        /// Lấy video theo catid và top
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public DataTable GetNewsVideoByCatAndTop(int Top, int catId)
        {
            IDbCommand cmd = _db.CreateCommand("TTOL_SelectVideoByCat", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }


        /// <summary>
        /// Lấy video top
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="catId"></param>
        /// <returns></returns>
        public DataTable GetListVideoAndTop(int pageIndex, int pageSize, int catId, long newsId)
        {
            IDbCommand cmd = _db.CreateCommand("TTOL_SelectAllVideo", true);
            _db.AddParameter(cmd, "PageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "PageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            _db.AddParameter(cmd, "newsId", DbType.Int64, newsId);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }

        public DataTable TinXuatBanTruoc(long News_ID, int CatID, int Top)
        {
            IDbCommand cmd = _db.CreateCommand("v2_fe_TinXuatBanTruoc", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, CatID);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }

        public DataTable TinXuatBanSau(long News_ID, int CatID, int Top)
        {
            IDbCommand cmd = _db.CreateCommand("v2_fe_TinXuatBanSau", true);
            _db.AddParameter(cmd, "News_ID", DbType.Int64, News_ID);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, CatID);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }

        public DataTable GetListTinDocNhieuNhat(int Top, int catId)
        {
            IDbCommand cmd = _db.CreateCommand("pr_DocNhieuNhatChuyenMuc", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            _db.AddParameter(cmd, "catId", DbType.Int32, catId);
            DataTable _dt = CreateDataTable(cmd);
            return _dt;
        }

        public DataTable GetListTinPhanHoiNhieu(int Top, int Cat_ID)
        {
            IDbCommand cmd = _db.CreateCommand("ttol_GetPhanHoiNhieuNhatChuyenMuc", true);
            _db.AddParameter(cmd, "Top", DbType.Int32, Top);
            _db.AddParameter(cmd, "CatID", DbType.Int32, Cat_ID);
            DataTable dataTable = CreateDataTable(cmd);
            return dataTable;
        }

        public DataTable GetPhotobyNewsID(string News_ID)
        {
            IDbCommand cmd = _db.CreateCommand("huan_GetPhoto_by_NewsID", true);
            _db.AddParameter(cmd, "News_ID", DbType.String, News_ID);
            DataTable __dt = CreateDataTable(cmd);
            return __dt;
        }

        /// <summary>
        /// Get tất cả Thread
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public DataTable GetAllThreadNews(int pageIndex, int pageSize)
        {
            IDbCommand cmd = _db.CreateCommand("ttol_GetThreadAllNew", true);
            _db.AddParameter(cmd, "PageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "PageSize", DbType.Int32, pageSize);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }


        /// <summary>
        /// Get Thread
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public DataTable GetThreadByThreadId(int threadId)
        {
            IDbCommand cmd = _db.CreateCommand("ttol_GetThreadByThreadId", true);
            _db.AddParameter(cmd, "ThreadID", DbType.Int32, threadId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        /// <summary>
        /// Get tất cả Thread khác một threadid nào đó
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public DataTable GetAllThreadNewsOtherId(int pageIndex, int pageSize, int threadId)
        {
            IDbCommand cmd = _db.CreateCommand("ttol_GetThreadAllOtherId", true);
            _db.AddParameter(cmd, "PageIndex", DbType.Int32, pageIndex);
            _db.AddParameter(cmd, "PageSize", DbType.Int32, pageSize);
            _db.AddParameter(cmd, "ThreadID", DbType.Int32, threadId);
            DataTable table = _db.CreateDataTable(cmd);
            return table;
        }

        /// <summary>
        /// Lấy tin Cat theo cat hiển thị
        /// </summary>
        /// <param name="Cat_DisplayURL"></param>
        /// <returns></returns>
        public DataTable GetCatIDByCatDisplayUrl(string Cat_DisplayURL)
        {
            IDbCommand cmd = _db.CreateCommand("v2_fe_GetCatIDByCatDisplayUrl", true);
            _db.AddParameter(cmd, "Cat_DisplayURL", DbType.String, Cat_DisplayURL);
            DataTable __dt = CreateDataTable(cmd);
            return __dt;
        }

        /// <summary>
        /// Lấy tin Cat theo cat hiển thị
        /// </summary>
        /// <param name="Cat_DisplayURL"></param>
        /// <returns></returns>
        public DataTable TTOL_GetCatIDByCatDisplayUrl(string Cat_DisplayURL)
        {
            IDbCommand cmd = _db.CreateCommand("TTOL_GetCatIDByCatDisplayUrl", true);
            _db.AddParameter(cmd, "Cat_DisplayURL", DbType.String, Cat_DisplayURL);
            DataTable __dt = CreateDataTable(cmd);
            return __dt;
        }

        public DataTable SelectVoteByCatId(int Cat_ID)
        {
            IDbCommand cmd = _db.CreateCommand("v2_fe_Vote", true);
            _db.AddParameter(cmd, "Cat_ID", DbType.Int32, Cat_ID);
            DataTable dataTable = CreateDataTable(cmd);
            return dataTable;
        }

        public void cms_Vote_Vote(int VoteIt_ID)
        {
            IDbCommand cmd = _db.CreateCommand("cms_Vote_Vote", true);
            _db.AddParameter(cmd, "voteItemId ", DbType.Int32, VoteIt_ID);
            CreateDataTable(cmd);
        }

        public void Upload_Insert(UploadEntity ue)
        {
            IDbCommand cmd = _db.CreateCommand("Web_TTOL_Upload_Insert", true);
            _db.AddParameter(cmd, "Upload_User", DbType.String, ue.Upload_User);
            _db.AddParameter(cmd, "Upload_Content", DbType.String, ue.Upload_Content);
            _db.AddParameter(cmd, "Upload_Date", DbType.DateTime, ue.Upload_Date);
            _db.AddParameter(cmd, "Upload_Email", DbType.String, ue.Upload_Email);
            _db.AddParameter(cmd, "Avatar", DbType.String, ue.Avatar);
            DataTable table = _db.CreateDataTable(cmd);
        }
    }
} // End of namespace


