export interface UserDto {
  id: string;
  firstName?: string;
  lastName?: string;
  username?: string;
  email?: string;
  bio?: string;
  isOnline?: boolean | null;
  accountCreatedAt?: Date;
}
