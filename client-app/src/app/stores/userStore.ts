import { observable, action, runInAction } from "mobx";
import { AuthService } from "../api/auth";
import { User } from "oidc-client";
import { IProfile } from "../models/profile";
import { createContext } from "react";
import agent from "../api/agent";


class UserStore {

    authService = new AuthService();
    @observable currentUser : User | null = null;
    @observable userProfile : IProfile | null = null;


    @action loadUser = () => {
        this.authService.getUser().then(user => {
            if(user) {
                this.currentUser = user;
                
                agent.profile.get(user.profile.sub).then(profile => {
                    if(profile) {
                        runInAction(() => {
                            this.userProfile = profile;
                            console.log(profile);
                        })
                    }
                }, error => {
                    console.log(error);
                });
            }
        });
    }
}

export default createContext(new UserStore());