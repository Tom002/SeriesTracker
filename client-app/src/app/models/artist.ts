export interface IArtist {
    artistId: number,
    name: string,
    birthDate: Date | null,
    deathDate: Date | null,
    city: string,
    imageUrl: string,
    about: string,
    writerOfCount: number,
    appearedInCount: number
}

export interface ICastMember {
    artistId: number,
    name: string,
    roleName: string,
    order: number,
    imageUrl: string
}

export interface IArtistDetails {
    artistId: number,
    name: string,
    birthDate?: Date,
    deathDate?: Date,
    city: string,
    imageUrl: string,
    about: string,
    writerOf: IWriterOfSeries[],
    appearedIn: IActorInSeries[]
}

export interface IWriterOfSeries {
    seriesId: number,
    title: string,
    coverImageUrl: string
}

export interface IActorInSeries {
    seriesId: number,
    title: string,
    coverImageUrl: string,
    roleName: string
}