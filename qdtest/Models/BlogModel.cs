using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace qdtest.Models
{
    public class PermissionModel
    {
        [Key]
        public int id { get; set; }
        public String name { get; set; }
        public Boolean enable { get; set; }
        public virtual List<GroupIDModel> group_ids { get; set; }
    }
    public class GroupIDModel
    {
        [Key]
        public int id { get; set; }
        //public String name { get; set; }
        public virtual List<PermissionModel> permisions { get; set; }
        public virtual List<UserModel> users { get; set; }
    }
    public class UserModel
    {
        [Key]
        public int id { get; set; }
        public String username { get; set; }
        public String password { get; set; }
        public Boolean sex { get; set; }
        public virtual GroupIDModel group_id { get; set; }
        public virtual List<PostModel> posts { get; set; }
        public virtual List<CategoryModel> categories { get; set; }
    }
    public class PostModel
    {
        [Key]
        public int id { get; set; }
        public String title { get; set; }
        public String content { get; set; }
        public virtual UserModel user { get; set; }
        public virtual List<CategoryModel> categories { get; set; }
        public virtual List<CommentModel> comments { get; set; }
    }
    public class CategoryModel
    {
        [Key]
        public int id { get; set; }
        public String name { get; set; }
        public String vdthoi { get; set; }
        public virtual UserModel user { get; set; }
        public virtual CategoryModel parent { get; set; }
        public virtual List<CategoryModel> childs { get; set; }
        public virtual List<PostModel> posts { get; set; }
    }
    public class OptionModel
    {
        [Key]
        public int id { get; set; }
        public String key { get; set; }
        public String value { get; set; }
    }
    public class CommentModel
    {
        [Key]
        public int id { get; set; }
        public String title { get; set; }
        public String content { get; set; }
        //public String guest_email { get; set; }
        public virtual PostModel post { get; set; }
    }
    //define context
    public class BlogDBContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<OptionModel> Options { get; set; }
        public DbSet<PermissionModel> Permissions { get; set; }
        public DbSet<GroupIDModel> GroupIDs { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
    }
}