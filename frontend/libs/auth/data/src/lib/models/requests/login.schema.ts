import * as z from 'zod';
import { LoginRequest } from './login.request';

export const LoginRequestSchema: z.ZodType<LoginRequest> = z.object({
  username: z.string().nonempty(),
  password: z.string().nonempty(),
});
