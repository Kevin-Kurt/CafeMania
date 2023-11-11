<template>
    <div class="content">
        <div class="d-flex" style="height: 100%;">
            <div
                style="max-width: 500px; width: 100%; margin: auto; background-color: white; color: white; padding-block: 50px; border-radius: 30px; padding-inline: 20px;">
                <div v-if="!register">
                    <div class="header">
                        CaféMania
                    </div>
                    <div class="mb-7">
                        <v-text-field v-model="user.email" height="51" placeholder="E-mail" class="primary-input" ou
                            hide-details="auto" outlined>
                        </v-text-field>
                    </div>

                    <v-text-field v-model="user.password" :type="show2 ? 'text' : 'password'" @click:append="show2 = !show2"
                        :append-icon="show2 ? 'mdi-eye' : 'mdi-eye-off'" placeholder="Senha" class="primary-input"
                        hide-details="auto" outlined></v-text-field>
                    <div class="d-flex mt-4" style="justify-content: flex-end; color: #50297F;">
                        <div style="cursor: pointer;" @click="register = true">
                            Cadastre-se
                        </div>
                    </div>
                    <div class="d-flex" style="justify-content: center;">
                        <v-btn color="#50297F" :loading="loading" style="color: white; text-transform: none;" class="mt-4"
                            @click="loginMethod()">
                            Entrar
                        </v-btn>
                    </div>
                </div>
                <div v-if="register">
                    <v-form ref="form" v-model="valid" @submit.stop.prevent="decryptPasswordRetrivalHash()">
                        <div class="header">
                            Cadastro
                        </div>
                        <v-text-field v-model="user.email" :rules="emailRules" placeholder="Digite seu e-mail"
                            class="primary-input mb-4" hide-details="auto" outlined></v-text-field>
                        <v-text-field v-model="newPassword" :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
                            :type="show ? 'text' : 'password'" @click:append="show = !show" :rules="[
                                (v) => !!v || 'campo obrigatório',
                                (v) =>
                                    (/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)/.test(v) &&
                                        v.length > 5) ||
                                    'Sua senha deve conter ao menos 6 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter',
                            ]" placeholder="Digite seu senha" class="primary-input mb-4" hide-details="auto"
                            outlined></v-text-field>
                        <v-text-field v-model="confirmPassword" :type="show1 ? 'text' : 'password'"
                            @click:append="show1 = !show1" :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'" :rules="[
                                (v) => !!v || 'campo obrigatório',
                                (v) =>
                                    (/(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*\W)/.test(v) &&
                                        v.length > 5) ||
                                    'Sua senha deve conter ao menos 6 caracteres, uma letra maiúscula, uma letra minúscula, um número e um caracter',
                            ]" placeholder="Confirme a senha" class="primary-input" hide-details="auto"
                            outlined></v-text-field>
                        <div class="d-flex mt-4" style="justify-content: flex-end; color: #50297F;">
                            <div style="cursor: pointer;" @click="register = false">
                                Entrar
                            </div>
                        </div>
                        <div class="d-flex" style="justify-content: center;">
                            <v-btn color="#50297F" style="color: white; text-transform: none;" class="mt-4"
                                @click="createAccount()">
                                Criar conta
                            </v-btn>
                        </div>
                    </v-form>
                </div>
            </div>

        </div>
    </div>
</template>
<script>
export default {
    name: "Login",
    components: {},
    data: () => ({
        user: {},
        loading: false,
        valid: false,
        show: false,
        show1: false,
        show2: false,
        register: false,
        newPassword: null,
        confirmPassword: null,
        emailRules: [
            (v) => !!v || "E-mail requerido",
            (v) => /.+@.+\..+/.test(v) || "E-mail inválido",
        ],
    }),
    watch: {
        newPassword() {
            this.$refs.form.validate();
        },
        confirmPassword() {
            this.$refs.form.validate();
        },
    },
    async created() {

    },

    methods: {
        createAccount() {
            if (this.$refs.form.validate()) {
                if (this.newPassword === this.confirmPassword) {
                    this.loading = true;
                    this.user.password = this.newPassword;

                    this.$axios.$post('user/add', this.user)
                        .then((res) => {
                            this.$store.commit(
                                "SET_LOGGED_USER",
                                JSON.stringify(res.content.user)
                            );
                            this.$store.commit("SET_CURRENT_TOKEN", res.content.token);
                            this.$store.commit("SET_EXPIRES_TOKEN", res.content.validTo);
                        })

                }
            }
        },

        async loginMethod() {
            this.loading = true
            await this.$store.dispatch("auth/login", this.user);
            this.loading = false
        },

    },
};
</script>

<style>
.header {
    color: #50297F;
    text-align: center;
    padding-bottom: 50px;
    font-size: 20px;
    font-weight: 600;
}

.content {
    height: 100%;
    background-image: linear-gradient(45deg,
            hsl(267deg 51% 33%) 0%,
            hsl(276deg 48% 34%) 5%,
            hsl(285deg 44% 35%) 10%,
            hsl(293deg 40% 36%) 14%,
            hsl(302deg 37% 38%) 19%,
            hsl(310deg 36% 41%) 24%,
            hsl(316deg 34% 44%) 29%,
            hsl(322deg 33% 47%) 34%,
            hsl(327deg 32% 51%) 38%,
            hsl(331deg 33% 54%) 43%,
            hsl(336deg 35% 57%) 48%,
            hsl(340deg 37% 60%) 53%,
            hsl(344deg 38% 63%) 58%,
            hsl(348deg 40% 65%) 62%,
            hsl(351deg 41% 68%) 67%,
            hsl(355deg 42% 71%) 72%,
            hsl(359deg 43% 74%) 77%,
            hsl(2deg 45% 76%) 81%,
            hsl(5deg 47% 78%) 86%,
            hsl(8deg 49% 80%) 91%,
            hsl(11deg 51% 83%) 95%,
            hsl(13deg 53% 85%) 100%) !important
}
</style>
