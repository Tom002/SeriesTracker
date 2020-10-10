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

export interface IReview {
    reviewerName: string,
    reviewerId: string,
    reviewTitle: string,
    reviewDate: Date,
    reviewText: string,
    rating: number
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


export interface ISeriesWatchedInfo {
    isWatchingSeries: boolean,
    hasLikedSeries: boolean,
    episodesWatchedIdList: number[]
}

export interface IUserSeriesReviewInfo {
    seriesId: number,
    isReviewedByUser: boolean,
    reviewTitle: string,
    reviewDate?: Date,
    reviewText: string,
    rating?: number  
}

export interface ISeriesReviewRequest {
    reviewTitle: string,
    reviewText: string
}

export interface ISeriesRateRequest {
    rating: number
}

export interface IEpisodeReviewRequest {
    rating: number
}

export interface IUserSeriesReview {
    seriesTitle: string,
    seriesId: number,
    reviewerId: string,
    reviewTitle: string,
    reviewDate: Date,
    reviewText: string,
    rating?: number
}

export interface ISeriesWatchedListItem {
    title: string,
    coverImageUrl: string,
    startYear: number,
    endYear?: number,
    seriesId: number
}

export interface ISeriesLikedListItem {
    title: string,
    coverImageUrl: string,
    startYear: number,
    endYear?: number,
    seriesId: number
}