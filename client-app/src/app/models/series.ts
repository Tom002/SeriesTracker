import { IArtist, ICastMember } from "./artist";

export interface ISeriesForList {
    seriesId: number,
    title: string,
    coverImageUrl: string,
    description: string,
    startYear: number | null,
    endYear: number | null,
    categories: ICategory[]
}

export interface ISeriesDetails {
    seriesId: number,
    title: string,
    coverImageUrl: string,
    description: string,
    startYear: number | null,
    endYear: number | null,
    ratingAverage: number,
    ratingsCount: number,
    seasonNumbers: number[],
    cast: ICastMember[],
    categories: ICategory[],
    writers: IArtist[],
    reviews: IReview[]
}

export interface ICategory {
    categoryId: number,
    categoryName: string
}

export interface IEpisode {
    episodeId: number,
    episodeTitle: string,
    description: string,
    season: number,
    episodeNumber: number,
    lengthInMinutes: number,
    release: Date | null,
    coverImageUrl: string,
}

export interface IEpisodeWatched {
    episodeId: number,
    watchingDate: Date
}

export interface ISeriesWatchedList {
    viewerId: string,
    seriesWatchedIds: number[]
}

export interface ISeriesEpisodesWatchedList {
    viewerId: string,
    seriesId: number,
    episodesWatchedIds: number[]
}

export interface IWatchSeriesRequest {
    viewerId: string,
    seriesId: number,
}

export interface IWatchEpisodeRequest {
    viewerId: string,
    episodeId: number,
    seriesId: number,
    watchingDate: Date,
    addToDiary: boolean
}

export interface IReviewSeriesRequest {
    reviewerId: string,
    seriesId: number,
    reviewTitle: string | null,
    reviewDate: Date | null,
    reviewText: string | null,
    rating: number | null
}

export interface IReviewEpisodeRequest {
    reviewerId: string,
    episodeId: number,
    rating: number | null
}

export interface IUserSeriesReviewInfo {
    seriesId: number,
    isReviewedByUser: boolean,
    reviewerId: string | null,
    reviewTitle: string | null,
    reviewDate: Date | null,
    reviewText: string | null,
    rating: number | null
}

export interface IReview {
    reviewerId: string,
    reviewTitle: string,
    reviewDate: Date,
    reviewText: string,
    rating: number
}