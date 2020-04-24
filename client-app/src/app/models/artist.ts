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