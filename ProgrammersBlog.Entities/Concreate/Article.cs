﻿using ProgrammersBlog.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.Concreate
{
    public class Article:EntityBase, IEntity
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public string Tumbnail { get; set; }
        public DateTime Date { get; set; }
        public int ViewsCount { get; set; }
        public int CommentCount { get; set; }
        public string SeoAuthor { get; set; }
        public string SeoDescription { get; set; }
        public string SeoTags { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }

        //SEO NEDİR?
        /*
         arama motoru optimizasyonu, web sitelerini arana motorlarının daha rahat bir şekilde anlayabilmesine "taramasına" olanak sağlayacak şekilde 
        arama motorlarının kriterlerine uygun hale getirerek "web sitesinin optimize edilmesi" hedeflenen anahtar kelimelere ait arama motoru aramalarında 
        yükseltilmesidir.
         
         */
    }
}
