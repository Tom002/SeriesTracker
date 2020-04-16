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