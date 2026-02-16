using AutoMapper;
using RecetArreAPI.DTOs.ApplicationUsers;
using RecetArreAPI.DTOs.Categories;
using RecetArreAPI.DTOs.Comments;
using RecetArreAPI.DTOs.Identity;
using RecetArreAPI.DTOs.Ingredients;
using RecetArreAPI.DTOs.Medals;
using RecetArreAPI.DTOs.Ratings;
using RecetArreAPI.DTOs.RecipeCategories;
using RecetArreAPI.DTOs.RecipeIngredients;
using RecetArreAPI.DTOs.Recipes;
using RecetArreAPI.DTOs.UserMedals;
using RecetArreAPI.DTOs.UserProfiles;
using RecetArreAPI.DTOs.WeeklyRankingEntries;
using RecetArreAPI.Models;

namespace RecetArreAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserCredentialsDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.Email));

            CreateMap<RegisterUserDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName));

            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationUserUpdateDto, ApplicationUser>();

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<CategoryUpdateDto, Category>();

            CreateMap<Comment, CommentDto>();
            CreateMap<CommentCreateDto, Comment>();
            CreateMap<CommentUpdateDto, Comment>();

            CreateMap<Ingredient, IngredientDto>();
            CreateMap<IngredientCreateDto, Ingredient>();
            CreateMap<IngredientUpdateDto, Ingredient>();

            CreateMap<Medal, MedalDto>();
            CreateMap<MedalCreateDto, Medal>();
            CreateMap<MedalUpdateDto, Medal>();

            CreateMap<Rating, RatingDto>();
            CreateMap<RatingCreateDto, Rating>();
            CreateMap<RatingUpdateDto, Rating>();

            CreateMap<Recipe, RecipeDto>();
            CreateMap<RecipeCreateDto, Recipe>();
            CreateMap<RecipeUpdateDto, Recipe>();

            CreateMap<RecipeCategory, RecipeCategoryDto>();
            CreateMap<RecipeCategoryCreateDto, RecipeCategory>();

            CreateMap<RecipeIngredient, RecipeIngredientDto>();
            CreateMap<RecipeIngredientCreateDto, RecipeIngredient>();
            CreateMap<RecipeIngredientUpdateDto, RecipeIngredient>();

            CreateMap<UserMedal, UserMedalDto>();
            CreateMap<UserMedalCreateDto, UserMedal>();
            CreateMap<UserMedalUpdateDto, UserMedal>();

            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileCreateDto, UserProfile>();
            CreateMap<UserProfileUpdateDto, UserProfile>();

            CreateMap<WeeklyRankingEntry, WeeklyRankingEntryDto>();
            CreateMap<WeeklyRankingEntryCreateDto, WeeklyRankingEntry>();
            CreateMap<WeeklyRankingEntryUpdateDto, WeeklyRankingEntry>();
        }
    }
}
