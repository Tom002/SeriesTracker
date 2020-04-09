import moment from 'moment'

export const ageFromDateOfBirth = (dateOfBirth: Date) : number => {
    return moment().diff(dateOfBirth, 'year')
}