module.exports = {
    content: [
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml',
        "./node_modules/flowbite/**/*.js"
    ],
    theme: {
        extend: {
            colors: {
                kprimary: {
                    DEFAULT: "#4b4bcc",
                    100: "#e0e0ff",
                    200: "#b3b3ff",
                    300: "#8080ff",
                    400: "#4d4dff",
                    500: "#1a1aff",
                    600: "#0000e6",
                    700: "#0000b3",
                    800: "#000080",
                    900: "#00004d"
                },
                ksecondary: {
                    DEFAULT: "#9292e8",
                    100: "#e6e6ff",
                    200: "#ccccff",
                    300: "#b3b3ff",
                    400: "#9999ff",
                    500: "#8080ff",
                    600: "#6666e6",
                    700: "#4d4db3",
                    800: "#333380",
                    900: "#1a1a4d"
                },
                kaccent: {
                    DEFAULT: "#ef9a5d",
                    100: "#fff0e6",
                    200: "#ffd9b3",
                    300: "#ffc280",
                    400: "#ffab4d",
                    500: "#ff941a",
                    600: "#e67a00",
                    700: "#b35f00",
                    800: "#804400",
                    900: "#4d2900"
                }
            },
            fontFamily: {
                sans: ['Commissioner', 'sans-serif', 'system-ui'],
                head: ['Inter', 'sans-serif', 'system-ui'],
            },
        }
    },
    plugins: [
        require('@tailwindcss/forms'),
        require('flowbite/plugin')
    ],
}
