import { IArtist } from "./artist";

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
    categories: ICategory[],
    episodes: IEpisode[],
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

export interface IReview {
    reviewerId: string,
    reviewTitle: string,
    reviewDate: Date,
    reviewText: string,
    rating: number
}