using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace BOATV
{
    public class SolrNewsItem
    {

        [SolrUniqueKey("News_Id")]
        public string NewsId { get; set; }

        [SolrField("News_Keywords")]
        public string NewsKeywords { get; set; }

        [SolrField("Cat_Id")]
        public string CatId { get; set; }

        [SolrField("News_Title")]
        public string Title { get; set; }

        [SolrField("News_Sapo")]
        public string Sapo { get; set; }

        [SolrField("News_Content")]
        public string Content { get; set; }

        [SolrField("News_Image")]
        public string Image { get; set; }

        [SolrField("News_PublishDate")]
        public DateTime PublishDate { get; set; }

        public float score { get; set; }

        public string Url { get; set; }

        public SolrNewsItem()
        {
        }
        public SolrNewsItem(string _id, string _title, string _sapo, string _image, DateTime _date, string _NewsKeywords, string catId, string content)
        {
            this.NewsId = _id;
            this.Title = _title;
            this.Sapo = _sapo;
            this.Image = _image;
            this.PublishDate = _date;
            this.NewsKeywords = _NewsKeywords;
            this.CatId = catId;
            this.Content = content;
            this.Url = string.Empty;
        }


    }
}
