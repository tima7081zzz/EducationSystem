import axios from '../axiosConfig';

const logout = async (): Promise<void> => {
    await axios.post(`/api/auth/logout`);
 };

export { logout };
