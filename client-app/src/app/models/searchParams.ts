export interface ISeriesParams {
    categoryId: number,
    titleFilter: string,
    sortBy: SeriesSortBy
}

export interface IArtistParams {
    nameFilter: string,
    occupation: ArtistOccupation,
    sort: ArtistSortBy
}

export interface IPagination {
    currentPage: number,
    itemsPerPage: number,
    totalItems: number,
    totalPages: number
}

export enum SeriesSortBy {
    HighestRating = 1,
    LowestRating = 2,
    MostReviewed = 3,
    Alphabetical = 4,
    AlphabeticalDescending = 5,
}

export enum ArtistSortBy {
    Alphabetical = 1,
    AlphabeticalDescending = 2
}

export enum ArtistOccupation {
    Actor = 1,
    Writer = 2,
    Both = 3,
    Any = 4
}

