import moment from "moment"
import React, { Fragment } from "react"
import { Header } from "semantic-ui-react"
import { IProfile } from "../../app/models/profile"

export const AboutUser : React.FC<{ profile: IProfile | null }> = ({profile}) => {
    return (
        <Fragment>
            <Header size='large'>
                About
            </Header>
            {profile?.about}

            <Header size='large'>
                Born
            </Header>
            {moment(profile?.birthDate).format("YYYY/MM/DD")}

            <Header size='large'>
                City
            </Header>
            {profile?.city}
        </Fragment>
        
    )
}