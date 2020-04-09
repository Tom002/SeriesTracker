
export interface ISeries {
    seriesId: number,
    title: string,
    coverImageUrl: string,
    description: string,
    startYear: number | null,
    endYear: number | null,
    categories: ICategory[]
}

export interface ICategory {
    categoryId: number,
    categoryName: string
}